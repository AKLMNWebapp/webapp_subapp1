using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.DAL;

namespace mvc.Controllers;

public class AllergyController : Controller
{
    private readonly ProductDbContext _context;

    public AllergyController (ProductDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var allergies = await _context.Allergies.ToListAsync();
        return View(allergies);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var allergy = await _context.Allergies
            .FirstOrDefaultAsync(a => a.AllergyCode == id);
        
        if (allergy == null)
        {
            return NotFound();
        }
        return View(allergy);
    }

    [HttpGet]
    public IActionResult CreateAllergy() 
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAllergy(Allergy allergy)
    {
        if (ModelState.IsValid)
        {
            _context.Allergies.Add(allergy);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ProductController");
        }

        return View(allergy);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var allergy = await _context.Allergies.FindAsync(id);
        if (allergy == null) NotFound();
        return View(allergy);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Allergy allergy)
    {
        if (ModelState.IsValid)
        {
            _context.Allergies.Update(allergy);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", nameof(Index));
        }
        return View(allergy);
    }

    [HttpGet]
    public async Task<IActionResult> Delete (int id)
    {
        var allergy = await _context.Allergies
            .FirstOrDefaultAsync(a => a.AllergyCode == id);
        
        if (allergy == null)
        {
            return NotFound();
        }
        return View(allergy);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var allergy = await _context.Allergies.FindAsync(id);
        if (allergy != null)
        {
            _context.Allergies.Remove(allergy);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}