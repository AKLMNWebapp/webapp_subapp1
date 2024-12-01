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

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Allergy> _allergyRepsitory;
    private readonly ILogger<CategoryController> _logger;
    public AdminController (UserManager<ApplicationUser> userManager, IRepository<Product> productRepository, IRepository<Allergy> allergyRepsitory, ILogger<CategoryController> logger)
    {
        _userManager = userManager;
        _productRepository = productRepository;
        _allergyRepsitory = allergyRepsitory;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index(string id)
    {
        var user =  await _userManager.FindByIdAsync(id);
        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> ListProducts()
    {
        //retrieve all products from db
        var allergies = await _allergyRepsitory.GetAll();
        var products = await _productRepository.GetAll();
        if (products == null)
        {
            _logger.LogError("[ProductController] product list not found while executing _productRepository.GetAll()");
            return NotFound("Product list not found");
        }
        //return the view with list of products
        var productViewModel = new ProductViewModel(products, "Index", allergies);
        return View(productViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            _logger.LogError("[AdminController] user not found while executing FindByIdAsync()");
            return NotFound("User not found");
        }
        return View(new ListUsersViewModel
        {
            user = user,
            Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
        });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmation(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if(user == null)
        {
            _logger.LogError("[AdminController] user not found while executing FindByIdAsync()");
            return NotFound ("User not found");
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("[AdminController] Failed to delete user with ID {UserId:0000}]", id);
            return StatusCode(500, "An error occurred while deleting the user.");
        }

        _logger.LogInformation("[AdminController] User with ID {UserId:0000} was successfully deleted.",id);
        return RedirectToAction(nameof(ListUsers)); // Redirect back to the user list
    }


    [HttpGet]
    public async Task<IActionResult> ListUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var listUsersViewList = new List<ListUsersViewModel>();

        foreach(var user in users)
        {
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            listUsersViewList.Add(new ListUsersViewModel
            {
                user = user,
                Role = role 
            });
        }
        return View(listUsersViewList);
    }

    
}