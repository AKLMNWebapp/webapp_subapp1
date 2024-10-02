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

    //Details() vil komme her, slik at man kan se hvilke produkter som tilhører hver kategori, men db må oppdateres, så venter i tilfelle det er flere ting i db som må oppdateres :c 

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