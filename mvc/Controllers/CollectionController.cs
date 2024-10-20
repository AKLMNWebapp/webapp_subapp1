
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.DAL;

namespace mvc.Controllers;

public class CollectionController : Controller
{
    private readonly IRepository<Collection> _colletionRepository;
    private readonly ILogger<CollectionController> _logger;

    public CollectionController (IRepository<Collection> collectionRepository, ILogger<CollectionController> logger)
    {
        _colletionRepository = collectionRepository;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collections = await _colletionRepository.GetAll();
        if(collections == null)
        {
            _logger.LogError("[CollectionController] collection list not found while executing GetAll()");
            return NotFound("Collection list not found");
        }
        return View(collections);
    }

    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var collection = await _colletionRepository.GetById(id);
        if (collection == null)
        {
            _logger.LogError("[CollecionController] collection not found for ID {CollectionId:0000}", id);
            return NotFound();
        }
        return View(collection);
    }
    

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        bool returnOk = await _colletionRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[CollectionController] collection not found for CollectionId {CollectionId:0000}", id);
            return BadRequest("Collection not found for the CollectionId");
        }
        return RedirectToAction(nameof(Index));
    }
}