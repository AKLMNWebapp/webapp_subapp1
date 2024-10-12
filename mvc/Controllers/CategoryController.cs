using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.Controllers;

public class CategoryController : Controller
{
    private readonly ProductDbContext _context;

    public CategoryController (ProductDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        //retrieve all products from db
        var categories = await _context.Categories.ToListAsync(); 
        //return the view with list of products
        return View(categories);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id); 

        if (category == null)
        {
            return NotFound();
        }
        return View(category); 
    }

    [HttpGet]
    public IActionResult CreateCategory() 
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        //find category by id
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound(); //return 404 if category not found
        }
        return View(category); //return delete confirmation view
    }

   [HttpPost]
   public async Task<IActionResult> DeleteConfirmation(int id)
   {
    //find category by id
    var category = await _context.Categories.FindAsync(id);
    if (category == null)
    {
        return NotFound(); //return 404 if category not found
        }
        _context.Categories.Remove(category); //remove category from db
        await _context.SaveChangesAsync(); //save changes to db 
        return RedirectToAction(nameof(Index)); //return view with updated list
    }
}