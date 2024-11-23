using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;
using mvc.DAL.Repositories;

namespace mvc.Controllers;

public class CollectionController : Controller
{
    private readonly IRepository<Collection> _collectionRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CollectionController> _logger;
    public CollectionController (IRepository<Collection> collectionRepository, IRepository<Product> productRepository, UserManager<ApplicationUser> userManager, ILogger<CollectionController> logger)
    {
        _collectionRepository = collectionRepository;
        _logger = logger;
        _productRepository = productRepository;
        _userManager = userManager;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collections = await _collectionRepository.GetAll();
        if(collections == null)
        {
            _logger.LogError("[CollectionController] collection list not found while executing GetAll()");
            return NotFound("Collection list not found");
        }
        return View(collections);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Collection collection)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _collectionRepository.Create(collection);
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[CollectionController] collection creation failed {@collection}", collection);
        return View(collection);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToCollection(int productId, int collectionId)
    {
        var product = await _productRepository.GetById(productId);
        var collection = await _collectionRepository.GetById(collectionId);

        if (product == null || collection == null)
        {
            _logger.LogError("Invalid product or collection ID.");
            return NotFound();
        }
        collection.Products.Add(product);
        await _collectionRepository.Update(collection);

        return RedirectToAction(nameof(Details), new { id = collectionId });
    }  
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var collection = await _collectionRepository.GetById(id);
        if (collection == null)
        {
            _logger.LogError("[CollectionController] Collection not found for CollectionId {CollectionId:0000}", id);
            return NotFound();
        }

        var products = await _productRepository.GetAll();
        ViewBag.Products = products;

        return View(collection);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        bool returnOk = await _collectionRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[CollectionController] collection not found for CollectionId {CollectionId:0000}", id);
            return BadRequest("Collection not found for the CollectionId");
        }
        return RedirectToAction(nameof(Index));
    }
}