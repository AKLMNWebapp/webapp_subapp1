using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;
using mvc.DAL.Repositories;
using System.Security.Claims;

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
    public async Task<IActionResult> Index(string id)
    {
        var collectionRepository = _collectionRepository as CollectionRepository; // casting to get methods that are not in interface
        if (collectionRepository == null)
        {
            _logger.LogError("[UserController] Unable to cast _CollectionRepository to collectionRepository");
            return StatusCode(500, "Internal server error");
        }

        var collections = await collectionRepository.GetAllByUserId(id);
        if(collections == null)
        {
            _logger.LogError("[BusinessController] products not found for UserId {UserId:0000}", id);
            collections = new List<Collection>();
       }

       return View(collections);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Business, User")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Business, User")]
    public async Task<IActionResult> Create(Collection collection)
    {
         // retrieve UserId 
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(userId))
        {
            collection.UserId = userId;

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                collection.User = user;
            }
            else
            {
                _logger.LogError("User with ID {UserId:0000} not found", userId);
                ModelState.AddModelError("", "Unable to find the associated user.");
                return View(collection);
            }
        }
        else
        {
            _logger.LogError("User ID is null or empty. Cannot create collection.");
            ModelState.AddModelError("", "Unable to determine the UserId for the current user.");
            return View(collection);
        }

        // create  collection
        bool returnOk = await _collectionRepository.Create(collection);

        if (returnOk)
        {
            return RedirectToAction(nameof(Index));
        }

        // log error if creation failed
        _logger.LogError("[CollectionController] Collection creation failed {@collection}", collection);
        ModelState.AddModelError("", "Failed to create the collection. Please try again.");
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
    [Authorize(Roles = "Admin, Business, User")]
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