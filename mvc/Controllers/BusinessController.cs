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

[Authorize(Roles = "Admin, Business")]
public class BusinessController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Product> _productRepository;
    private readonly ILogger<CategoryController> _logger;
    public BusinessController (UserManager<ApplicationUser> userManager, IRepository<Product> productRepository, ILogger<CategoryController> logger)
    {
        _userManager = userManager;
        _productRepository = productRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            _logger.LogError("[BusinessController] User not found while executing FindByIdAsync() for UserID {UserId}", id);
            return NotFound("User not found");
        }

        var productRepository = _productRepository as ProductRepository; // casting to get methods that are not in interface
        if (productRepository == null)
        {
            _logger.LogError("[BusinessController] Unable to cast _productRepository to ProductRepository");
            return StatusCode(500, "Internal server error");
        }

        var products = await productRepository.GetAllByUserId(id);
        if(products == null)
        {
            _logger.LogError("[BusinessController] products not found for UserId {UserId:0000}", id);
            products = new List<Product>();
        }

        var businessProductViewModel = new BusinessProductViewModel(products, "ListProducts", user);

       return View(businessProductViewModel);
    }
}