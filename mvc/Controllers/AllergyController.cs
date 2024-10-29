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
        _allergyRepository = allergyRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var allergies = await _allergyRepository.GetAll();
        if (allergies == null)
        {
            _logger.LogError("[AllergyController] allergy list not found while executing _allergyRepository.GetAll()");
            return NotFound("allergy list not found");
        }
        //return the view with list of allergies
        return View(allergies);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var allergy = await _allergyRepository.GetById(id);
        if (allergy == null)
        {
            _logger.LogError("[AllergyController] allergy not found for allergyID {allergyID:0000}", id);
            return BadRequest("Allergy not found for allergyID");
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
            bool returnOk = await _allergyRepository.Create(allergy);
            if (returnOk)
                return RedirectToAction(nameof(Index));       
        }
        _logger.LogError("[AllergyController] allergy creation failed {@category}", allergy);
        return View(allergy);
    }

        [HttpGet]
    public async Task<IActionResult> Update(int id) 
    {
        var allergy = await _allergyRepository.GetById(id);
        if(allergy == null)
        {
            _logger.LogError("[AllergyController] Category not found for CategoryId {CategoryId:0000}", id);
            return BadRequest("Category not found for the categoryId");
        }
        return View(allergy);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Allergy allergy)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _allergyRepository.Update(allergy);
            if (returnOk)
                return RedirectToAction(nameof(Index));       
        }
        _logger.LogWarning("[AllergyController] allergy update failed {@allergy}", allergy);
        return View(allergy);
    }
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {   
        //find category by id
        var allergy = await _allergyRepository.GetById(id);
        if (allergy == null)
        {
            _logger.LogError("[AllergyController] allergy not found for AllergyID {AllergyID:0000}", id);
            return NotFound(); //return 404 if category not found
        }
        return View(allergy); //return delete confirmation view
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var allergy = await _allergyRepository.GetById(id);
        if (allergy != null)
        {
            await _allergyRepository.Delete(id);
        }
        return RedirectToAction(nameof(Index)); //return view with updated list
    }
}