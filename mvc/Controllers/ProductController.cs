using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.ViewModels;
using mvc.Models;

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

    [HttpGet]
    public IActionResult CreateProduct() 
    {
        return View();
    }

    /*[HttpPost]
    public async Task<IActionResult> CreateProduct()
    {
        var allergies = await _context.Allergies.ToListAsync();
        var createProductViewModel = new CreateProductViewModel
        {
           product = new Product(),

           AllergyMultiSelectList = 
        }
    }*/
    
    /*
    [HttpGet]
    //request the products table for a product with the specified ProductId, including related Reviews and Users
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .Include(p => p.Reviews) //includes the Reviews related to the product
            .ThenInclude (r => r.User) //for each review, also include the user
            .FirstOrDefaultAsync(p => p.ProductId == id); //will be updated later when user authorization is set

        //check if the product was not, if not 404
        if (product == null)
        {
            return NotFound();
        }
        return View(product); //if product found, return view for the product
    }
    */

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