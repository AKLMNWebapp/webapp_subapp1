
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.DAL;

namespace mvc.Controllers;

public class CollectionController : Controller
{
    private readonly ProductDbContext _context;

    public CollectionController (ProductDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collections = await _context.Collections.ToListAsync();
        return View(collections);
    }

    /*
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var collection = await _context.Collections
            .FirstOrDefaultAsync(c => c.CollectionId == id);
            
        if (collection == null)
        {
            return NotFound();
        }
        return View(collection);
    }
    */
    

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var collection = await _context.Collections.FindAsync(id);
        if (collection == null)
        {
            return NotFound();
        }
        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
}