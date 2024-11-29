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

namespace mvc.Controllers;

public class ReviewController : Controller
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController (IRepository<Review> reviewRepository, IRepository<Product> productRepository, UserManager<ApplicationUser> userManager, ILogger<ReviewController> logger)
    {
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
        _userManager = userManager;
        _logger = logger;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var reviews = await _reviewRepository.GetAll();
        if (reviews == null)
        {
            _logger.LogError("[ReviewController] review list not found while executing GetAll()");
            reviews = new List<Review>();
        }
        return View(reviews);
    }

    [HttpGet]
    public async Task<IActionResult> ListReviews(int id) {
        var product = await _productRepository.GetById(id);
        if (product == null)
        {
            _logger.LogError("[ReviewController] product not found for ProductId {ProductId:0000}", id);
            return NotFound();
        }

        var reviewRepository = _reviewRepository as ReviewRepository; // casting to get methods that are not in interface
        if (reviewRepository == null)
        {
            _logger.LogError("[ReviewController] Unable to cast _reviewRepository to ReviewRepository");
            return StatusCode(500, "Internal server error");
        }
       
       var reviews = await reviewRepository.GetAllByProductId(id);
       if ( reviews == null || !reviews.Any()) 
       {
            _logger.LogError("[ReviewController] Reviews not found for ProductId {ProductId:0000}", id);
            reviews = new List<Review>(); // sends an empty list if no reviews
       }

       ViewBag.ProductId = id;
       return View(reviews);
    }
    

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> CreateReview(int id)
    {
        var product = await _productRepository.GetById(id);
        if (product == null)
        {
            _logger.LogError("[ProductController] product not found for ProductId {ProductId:0000}", id);
            return NotFound("Product not found");
        }
        var review = new Review
        {
            ProductId = id
        };
        return View(review);
    }

   [HttpPost]
   [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> CreateReview(Review review)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId != null )
        {
            review.UserId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user != null) {
                review.User = user;
            }
            else 
            {
                _logger.LogError("[ProductController] User not found for user {user}", user);
                return NotFound("User not found");
            }
        } 
        else return Unauthorized();

        if (ModelState.IsValid)
        {

            bool returnOk = await _reviewRepository.Create(review);
            if(returnOk)
            {
                int productId = review.ProductId;
                _logger.LogInformation("[ReviewController] Review created successfully for ProductId {ProductId}", productId);
                return RedirectToAction("ListReviews", new { id = productId });
            }
            else
            {
                _logger.LogError("[ProductController] Failed to create review for ProductId {ProductId:0000}", review.ProductId);
                return BadRequest("Review creation failed");
            }     
        }
        return View(review);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null) NotFound();
        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Review review)
    {
        if (ModelState.IsValid)
        {
            var returnOk = await _reviewRepository.Update(review);
            if (returnOk)
            {
                int productId = review.ProductId;
                _logger.LogInformation("[ReviewController] Successfully updated review with ReviewId {ReviewId}", review.ReviewId);
                return RedirectToAction("Details", "Product", new {id = productId});
            }
            else
            {
                _logger.LogError("[ReviewController] failed to update review with ReviewId {ReviewId}", review.ReviewId);
            }
        }
        return View(review);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null)
        {
            _logger.LogError("[ReviewController] review not found for ReviewId {ReviewId:0000}", id);
            return BadRequest("Review not found for the ReviewId");
        }
        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null) return NotFound();
        string userId = review.UserId;
        var success = await _reviewRepository.Delete(id);
        if (success)
        {
            int productId = review.ProductId;
            _logger.LogInformation("[ReviewController] Successfully deleted review with ReviewId {ReviewId}", id);
            return RedirectToAction("ListReviews", new {id = productId});
        }
        else
        {
            _logger.LogError("[ReviewController] Failed to delete review with ReviewId {ReviewId}", id);
            return View(review);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Respond(int id)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null) {
            _logger.LogError("[ReviewController] review not found for ReviewId {ReviewId:0000}", id);
            return BadRequest("Review not found for the ReviewId");
        }

        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> Respond(int id, string response)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null) {
            _logger.LogError("[ReviewController] review not found for ReviewId {ReviewId:0000}", id);
            return BadRequest("Review not found for the ReviewId");
        }

        review.Response = response;

        // returns current userID
        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userID == null) return Unauthorized();

        review.ResponseUserID = userID;

        var success = await _reviewRepository.Update(review);
        if (success)
        {
            int productId = review.ProductId;
            _logger.LogInformation("[ReviewController] Successfully added review response to review with ReviewId {ReviewId}", review.ReviewId);
            return RedirectToAction("ListReviews", new {id = productId});
        }
        else
        {
            _logger.LogError("[ReviewController] Failed to add response to review with ReviewId {ReviewId:0000}", id);
            return BadRequest("Failed to add response");
        }

    }

    [HttpGet]
    public async Task<IActionResult> DeleteResponse(int id)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null)
        {
            _logger.LogError("[ReviewController] review not found for ReviewId {ReviewId:0000}", id);
            return BadRequest("Review not found for the ReviewId");
        }

        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteResponseConfirm(int id)
    {
        var review = await _reviewRepository.GetById(id);
        if (review == null)
        {
            _logger.LogError("[ReviewController] review not found for ReviewId {ReviewId:0000}", id);
            return BadRequest("Review not found for the ReviewId");
        }

        review.Response = "";

        var success = await _reviewRepository.Update(review);
        if (success)
        {
            int productId = review.ProductId;
            _logger.LogInformation("[ReviewController] Successfully deleted review response to review with ReviewId {ReviewId}", review.ReviewId);
            return RedirectToAction("ListReviews", new {id = productId});
        }
        else
        {
            _logger.LogError("[ReviewController] Failed to delete response to review with ReviewId {ReviewId:0000}", id);
            return BadRequest("Failed to delete response");
        }
    }
}