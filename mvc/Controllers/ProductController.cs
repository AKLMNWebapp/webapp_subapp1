using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.ViewModels;
using mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using mvc.DAL;
using Microsoft.AspNetCore.Http.HttpResults;

namespace mvc.Controllers;

public class ProductController : Controller
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Allergy> _allergyRepsitory;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IRepository<Product> productRepository, IRepository<Allergy> allergyRepsitory, ILogger<ProductController> logger)
    {
        _productRepository = productRepository; //initialize the db
        _allergyRepsitory = allergyRepsitory;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        //retrieve all products from db
        var products = await _productRepository.GetAll();
        if (products == null)
        {
            _logger.LogError("[ProductController] product list not found while executing _productRepository.GetAll()");
            return NotFound("Product list not found");
        }
        //return the view with list of products
        return View(products);
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
    public async Task<IActionResult> CreateProduct() 
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

        return View(createProductViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
    {
        if(ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(model.NewAllergyName))
            {
                var newAllergy = new Allergy {Name = model.NewAllergyName};
                bool allergyCreated = await _allergyRepsitory.Create(newAllergy);

                if (allergyCreated)
                {
                    model.SelectedAllergyCodes.Add(newAllergy.AllergyCode);
                }
            }


            foreach ( var allergyCode in model.SelectedAllergyCodes)
            {
                model.Product.AllergyProducts.Add(new AllergyProduct {
                    AllergyCode = allergyCode,
                    Product = model.Product
                });
            }

            bool productCreated = await _productRepository.Create(model.Product);
            if (productCreated)
            {
                 _logger.LogInformation("[ProductController] product created successfully for {@product}", model.Product);
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError("[ProductController] product creation failed for {@model}", model);
                return BadRequest("Product creation failed");
            }
        }
        return View(model);
    }

    [HttpGet]
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
    public async Task<IActionResult> Update(CreateProductViewModel model) 
    {
        var product = model.Product;
        if (ModelState.IsValid)
        {

            if (!string.IsNullOrEmpty(model.NewAllergyName))
            {
                var newAllergy = new Allergy {Name = model.NewAllergyName};
                bool allergyUpdated = await _allergyRepsitory.Update(newAllergy);

                if (allergyUpdated)
                {
                    model.SelectedAllergyCodes.Add(newAllergy.AllergyCode);
                }
            }

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
   