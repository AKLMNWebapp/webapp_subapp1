
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;
using mvc.DAL.Repositories;

namespace mvc.Controllers;

public class CollectionController : Controller
{
    private readonly IRepository<Collection> _colletionRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly ILogger<CollectionController> _logger;

    public CollectionController (IRepository<Collection> collectionRepository, ILogger<CollectionController> logger, IRepository<Product> productRepository)
    {
        _colletionRepository = collectionRepository;
        _logger = logger;
        _productRepository = productRepository;
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
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Collection collection)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _colletionRepository.Create(collection);
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[CollectionController] collection creation failed {@collection}", collection);
        return View(collection);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToCollection(int productId, string? collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            ModelState.AddModelError("CollectionName", "Collection name cannot be empty.");
            return BadRequest("Invalid input.");
        }

        // check if the product exists
        var product = await _productRepository.GetById(productId);
        if (product == null)
        {
            _logger.LogError("[CollectionController] Product not found for ProductId {ProductId:0000}", productId);
            return NotFound("Product not found.");
        }

        // check if the collection already exists
        var collections = await _colletionRepository.GetAll();
        var collection = collections.FirstOrDefault(c => c.Name == collectionName);

        if (collection == null)
        {
            // create a new collection
            collection = new Collection
            {
                Name = collectionName,
                CreatedAt = DateTime.Now
            };

            // add the new collection
            bool collectionCreated = await _colletionRepository.Create(collection);
            if (!collectionCreated)
            {
                _logger.LogError("[CollectionController] Collection creation failed for {CollectionName}", collectionName);
                return BadRequest("Failed to create the collection.");
            }

            _logger.LogInformation("[CollectionController] Collection {CollectionName} created successfully.", collectionName);
        }

        // add the product to the collection if not already added
        if (!collection.Products.Any(p => p.ProductId == productId))
        {
            collection.Products.Add(product);
            await _colletionRepository.Update(collection); // update collection with the product 
        }
        return RedirectToAction("Details", new { id = collection.CollectionId });
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

        var products = await _productRepository.GetAll();
        ViewBag.Products = products;
        
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