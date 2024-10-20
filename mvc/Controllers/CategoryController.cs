using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.DAL;

namespace mvc.Controllers;

public class CategoryController : Controller
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController (IRepository<Category> categoryRepository, ILogger<CategoryController> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        //retrieve all products from db
        var categories = await _categoryRepository.GetAll(); 
        //return the view with list of products
        if (categories == null)
        {
            _logger.LogError("[CategoryController] category list not found while executing GetAll()");
            return NotFound("Category list not found");
        }
        return View(categories);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var category = await _categoryRepository.GetById(id);

        if (category == null)
        {
            _logger.LogError("[CategoryController] Category not found for CategoryId {CategoryId:0000}",id);
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
            bool returnOk = await _categoryRepository.Create(category);
            if (returnOk)
                return RedirectToAction(nameof(Index));       
        }
        _logger.LogError("[CategoryController] category creation failed {@category}", category);
        return View(category);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        //find category by id
        var category = await _categoryRepository.GetById(id);
        if (category == null)
        {
            _logger.LogError("[CategoryController] Category not found for CategoryId {CategoryId:0000}", id);
            return NotFound(); //return 404 if category not found
        }
        return View(category); //return delete confirmation view
    }

   [HttpPost]
   public async Task<IActionResult> DeleteConfirmation(int id)
   {
    //find category by id
    bool returnOk = await _categoryRepository.Delete(id);
    if (!returnOk)
    {
        _logger.LogError("[CategoryController] category deletion failed for CategoryId {CategoryId:0000}",id);
        return BadRequest("Category not found for the CategoryId"); //return 404 if category not found
        }
        return RedirectToAction(nameof(Index)); //return view with updated list
    }
}