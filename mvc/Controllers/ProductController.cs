using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc.DAL.ViewModels;
using mvc.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using mvc.DAL.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace mvc.Controllers;

public class ProductController : Controller
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Allergy> _allergyRepsitory;
    private readonly IRepository<Category> _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IRepository<Product> productRepository, IRepository<Allergy> allergyRepsitory, IRepository<Category> categoryRepository, UserManager<ApplicationUser> userManager, ILogger<ProductController> logger)
    {
        _productRepository = productRepository; //initialize the db
        _allergyRepsitory = allergyRepsitory;
        _categoryRepository = categoryRepository;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        //retrieve all products from db
        var allergies = await _allergyRepsitory.GetAll();
        var products = await _productRepository.GetAll();
        if (products == null)
        {
            _logger.LogError("[ProductController] product list not found while executing _productRepository.GetAll()");
            return NotFound("Product list not found");
        }
        //return the view with list of products
        var productViewModel = new ProductViewModel(products, "Index", allergies);
        return View(productViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productRepository.GetById(id);
        if (product == null)
        {
            _logger.LogError("[ProductController] product not found for ProductId {ProductId:0000}", id);
            return BadRequest("Product not found for ProductId");
        }
        return View(product);
    }


    // This Get request populates the Allergy section with already existing allergies in the database
    [HttpGet]
    [Authorize(Roles = "Admin, Business")]
    public async Task<IActionResult> CreateProduct() 
    {
        var allergies = await _allergyRepsitory.GetAll(); // gets list of all available allergies
        var categories = await _categoryRepository.GetAll();

        // Our viewModel here is used to list all allergies in our select menu on the view
        var createProductViewModel = new CreateProductViewModel
        {
            Product = new Product(),
            AllergyMultiSelectList = allergies.Select(allergy => new SelectListItem {
                Value = allergy.AllergyCode.ToString(),
                Text = allergy.Name
            }).ToList(),

            CategorySelectList = categories.Select(cateorgy => new SelectListItem {
                Value = cateorgy.CategoryId.ToString(),
                Text = cateorgy.Name
            }).ToList()
        };

        return View(createProductViewModel);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Business")]
    public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                model.Product.UserId = userId;
                var user = await _userManager.FindByIdAsync(userId);
                model.Product.User = user;
            }
        if (!ModelState.IsValid)
    {
        // Log all ModelState errors
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            _logger.LogError($"ModelState Error: {error.ErrorMessage}");
        }
        return View(model);  // Return the view with model so that errors can be displayed
    }
        if(ModelState.IsValid)
        {

            model.Product.CreatedAt = DateTime.Now;

            foreach ( var allergyCode in model.SelectedAllergyCodes)
            {
                var newAllergy = await _allergyRepsitory.GetById(allergyCode);
                if (newAllergy != null)
                {
                    var newAllergyProduct = new AllergyProduct
                    {
                        AllergyCode = allergyCode,
                        Allergy = newAllergy,
                        ProductId = model.Product.ProductId,
                        Product = model.Product
                    };

                    model.Product.AllergyProducts.Add(newAllergyProduct);
                }
            }

            bool productCreated = await _productRepository.Create(model.Product);
            Console.WriteLine("Creating product: " + model.Product.Name);
            if (productCreated)
            {
                Console.WriteLine("Product saved!");
                _logger.LogInformation("[ProductController] product created successfully for {@product}", model.Product);
                return RedirectToAction("Index");
            }
        }
        return BadRequest("Product creation failed");
    }

    [HttpGet]
    public IActionResult CreateNewAllergy()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewAllergy(Allergy allergy) {

        if (ModelState.IsValid)
        {
            bool allergyCreated = await _allergyRepsitory.Create(allergy);
            if (allergyCreated) 
            {
                var allergies = await _allergyRepsitory.GetAll(); // gets list of all available allergies

                // Our viewModel here is used to list all allergies in our select menu on the view
                var createProductViewModel = new CreateProductViewModel
                {
                Product = new Product(),
                AllergyMultiSelectList = allergies.Select(allergy => new SelectListItem {
                    Value = allergy.AllergyCode.ToString(),
                    Text = allergy.Name
                }).ToList()
            };

            return RedirectToAction("CreateProduct", createProductViewModel);
            }
                    
        }
        _logger.LogError("[AllergyController] category creation failed {@allergy}", allergy);
        return BadRequest("Allergy creation failed");
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Business")]
    public async Task<IActionResult> Update(int id) {

        // Find the specific product from id
        var product = await _productRepository.GetById(id); // uses repo method

        // product not found
        if (product == null)
        {
            _logger.LogError("[ProductController] product not found when updating the ProductId {ProductId:0000}", id);
            return BadRequest("Product not found for the ProductId");
        }
        
        // Fetch all existing allergies
        var allergies = await _allergyRepsitory.GetAll(); // gets list of all available allergies

        // Our viewModel here is used to list all allergies in our select menu on the view
        var updateProductViewModel = new CreateProductViewModel
        {
            Product = new Product(),
            AllergyMultiSelectList = allergies.Select(allergy => new SelectListItem {
                Value = allergy.AllergyCode.ToString(),
                Text = allergy.Name
            }).ToList()
        };

        return View(updateProductViewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Business")]
    public async Task<IActionResult> Update(CreateProductViewModel model) 
    {
        var product = model.Product;
        if (ModelState.IsValid)
        {

            model.Product.AllergyProducts.Clear(); // avoids duplicating allergyProducts

            // This loop iterates through all allergy codes that have been selected from the allergy menu
            // The selected allergies will be saved as an AllergyProduct
            foreach (var allergyCode in model.SelectedAllergyCodes)
            {
                product.AllergyProducts.Add(new AllergyProduct {
                    AllergyCode = allergyCode,
                    Product = product
                });
            }

            bool productUpdated = await _productRepository.Update(model.Product);
            if (productUpdated)
            {
                _logger.LogInformation("[ProductController] product updated successfully for ProductId {ProductId:0000}", product.ProductId);
                return RedirectToAction("Index"); // returns to index view
            }
            else
            {
                return BadRequest("Product creation failed");
            }
        }
        return View(product);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Business")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.GetById(id);
        if (product == null)
        {
            _logger.LogError("[ProductController] product not found for ProductId {ProductId:0000}", id);
            return BadRequest("Product not found for the ProductId");
        }
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Business")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _productRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[ProductController] product deletion failed for ProductId {ProductId:0000}", id);
            return BadRequest("Product not found for the ProductId");

        }
        return RedirectToAction(nameof(Index));
    }
}
   