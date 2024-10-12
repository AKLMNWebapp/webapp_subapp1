using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.ViewModels;
using mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace mvc.Controllers;

public class ProductController : Controller
{
    private readonly ProductDbContext _context;

    public ProductController(ProductDbContext context)
    {
        _context = context; //initialize the db
    }

    public async Task<IActionResult> Index()
    {
        //retrieve all products from db
        var products = await _context.Products.ToListAsync(); 
        //return the view with list of products
        return View(products);
    }

    // This Get request populates the Allergy section with already existing allergies in the database
    [HttpGet]
    public async Task<IActionResult> CreateProduct() 
    {
        var allergies = await _context.Allergies.ToListAsync(); // gets list of all available allergies

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
        var product = model.Product;
        if (ModelState.IsValid)
        {

            // This code ensures that the product can only be made by users, that are logged in
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userID))
            {
                return Forbid();
            }

            // This code handles user with self-made allergies that are not already pre existing in out database
            product.UserId = int.Parse(userID); // parses id to string to use in if statement
            if (!string.IsNullOrEmpty(model.NewAllergyName)) {
                var newAllergy = new Allergy {Name = model.NewAllergyName}; // Creates a new allergy
                _context.Allergies.Add(newAllergy);
                await _context.SaveChangesAsync(); // Saves new allergy to database
                model.SelectedAllergyCodes.Add(newAllergy.AllergyCode); // adds the new allergies codes to viewModel
            }

            // This loop iterates through all allergy codes that have been selected from the allergy menu
            // The selected allergies will be saved as an AllergyProduct
            foreach (var allergyCode in model.SelectedAllergyCodes)
            {
                product.AllergyProducts.Add(new AllergyProduct {
                    AllergyCode = allergyCode,
                    Product = product
                });
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // updated the db with new product
            return RedirectToAction("Index"); // redirects to Index view.
        }
        return View(product);
    }
    
    
    [HttpGet]
    //request the products table for a product with the specified ProductId, including related Reviews and Users
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            //.Include(p => p.Reviews) //includes the Reviews related to the product
            //.ThenInclude (r => r.User) //for each review, also include the user
            .FirstOrDefaultAsync(p => p.ProductId == id); //will be updated later when user authorization is set

        //check if the product was not, if not 404
        if (product == null)
        {
            return NotFound();
        }
        return View(product); //if product found, return view for the product
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        //find product by id
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(); //return 404 if product not found
        }
        return View(product); //return delete confirmation view
    }

   [HttpPost]
   public async Task<IActionResult> DeleteConfirmed(int id)
   {
    //find product by id
    var product = await _context.Products.FindAsync(id);
    if (product == null)
    {
        return NotFound(); //return 404 if product not found
        }
        _context.Products.Remove(product); //remove product from db
        await _context.SaveChangesAsync(); //save changes to db 
        return RedirectToAction(nameof(Index)); //return view with updated list
    }
}