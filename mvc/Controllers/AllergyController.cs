using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace mvc.Controllers;

public class AllergyController : Controller
{
    private readonly ProductDbContext _context;

    public AllergyController (ProductDbContext context)
    {
        _context = context;
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
}