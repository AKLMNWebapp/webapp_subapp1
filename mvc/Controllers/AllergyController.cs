using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc.DAL.ViewModels;
using mvc.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using mvc.DAL.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace mvc.Controllers;

public class AllergyController : Controller
{
    private readonly IRepository<Allergy> _allergyRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly ILogger<AllergyController> _logger;

    public AllergyController (IRepository<Allergy> allergyRepository, IRepository<Product> productRepository, ILogger<AllergyController> logger)
    {
        _allergyRepository = allergyRepository;
        _productRepository = productRepository;
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
    public IActionResult CreateNewAllergy()
    {
        // Retrieve the CreateProductViewModel from TempData
        var createProductViewModel = TempData["CreateProductViewModel"] as CreateProductViewModel;

        if (createProductViewModel == null)
        {
            // If the model was not found in TempData, you can handle this case (e.g., redirect to a different action).
            return RedirectToAction("CreateProduct", "Product", createProductViewModel);
        }

        // Pass the model to the view
        return View(createProductViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewAllergy(Allergy allergy)
    {

        if (ModelState.IsValid)
        {
            bool allergyCreated = await _allergyRepository.Create(allergy);
            if (allergyCreated)
            {
                var allergies = await _allergyRepository.GetAll(); // gets list of all available allergies

                // Our viewModel here is used to list all allergies in our select menu on the view
                // we use TempData to get back to the same product we were making

                if (TempData["CreateProductViewModel"] is string createProductViewModelJson)
                {
                    var createProductViewModel = JsonConvert.DeserializeObject<CreateProductViewModel>(createProductViewModelJson);
                    createProductViewModel.AllergyMultiSelectList = allergies.Select(a => new SelectListItem
                    {
                        Value = a.AllergyCode.ToString(),
                        Text = a.Name
                    }).ToList();

                    return RedirectToAction("CreateProduct", "Product", createProductViewModel);
                }
                else {
                    return BadRequest("Test failed");
                }
            }
            else return BadRequest("allergyCreated false");

        }
        else
    {
        // Log the errors in the ModelState
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            _logger.LogError("ModelState Error: {ErrorMessage}", error.ErrorMessage);
        }
    }
        _logger.LogError("[AllergyController] allergy creation failed {@allergy}", allergy);
        return BadRequest("Allergy creation failed");
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