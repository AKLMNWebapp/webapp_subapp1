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

[Authorize(Roles = "Admin, User")]
public class RegularUserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Review> _reviewRepository;
    private readonly ILogger<CategoryController> _logger;
    public RegularUserController (UserManager<ApplicationUser> userManager, IRepository<Review> reviewRepository, ILogger<CategoryController> logger)
    {
        _userManager = userManager;
        _reviewRepository = reviewRepository;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index(string id)
    {
        var user =  await _userManager.FindByIdAsync(id);
        return View(user);
    }
    [HttpGet]
    public async Task<IActionResult> ListReviews(string id)
    {
        var reviewRepository = _reviewRepository as ReviewRepository; // casting to get methods that are not in interface
        if (reviewRepository == null)
        {
            _logger.LogError("[RegularUserController] Unable to cast _reviewRepository to ReviewRepository");
            return StatusCode(500, "Internal server error");
        }

        var reviews = await reviewRepository.GetAllByUserId(id);
        if (reviews == null)
        {
            _logger.LogError("[RegularUserController] reviews not found for UserId {UserId:0000}", id);
            reviews = new List<Review>();
        }

        return View(reviews);
    }
}