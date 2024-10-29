using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.DAL;

namespace mvc.Controllers;

public class AllergyController : Controller
{
    private readonly IRepository<Allergy> _allergyRepository;
    private readonly ILogger<AllergyController> _logger;

    public AllergyController (IRepository<Allergy> allergyRepository, ILogger<AllergyController> logger)
    {
        _allergyRepository = allergyRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var allergies = await _allergyRepository.GetAll();
        if (allergies == null)
        {
            _logger.LogError("[AllergyController] allergy list not found while executing GetAll()");
            return NotFound("Allergy list not found");
        }
        return View(allergies);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var allergy = await _allergyRepository.GetById(id);

        if (allergy == null)
        {
             _logger.LogError("[AllergyController] Allergy not found for AllergyCode {AllergyCode:0000}", id);
            return NotFound();
        }
        return View(allergy);
    }
/*
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
*/
    [HttpGet]
    public async Task<IActionResult> Delete (int id)
    {
        var allergy = await _allergyRepository.GetById(id);
        
        if (allergy == null)
        {
            return NotFound();
        }
        return View(allergy);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var allergy = await _allergyRepository.GetById(id);
        if (allergy != null)
        {
            await _allergyRepository.Delete(id);
        }
        return RedirectToAction(nameof(Index));
    }
}