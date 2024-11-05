using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.DAL;
using System.Security.Claims;

namespace mvc.Controllers;

public class ReviewController : Controller
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController (IRepository<Review> reviewRepository, ILogger<ReviewController> logger)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        var reviews = await _reviewRepository.GetAll();
        if (reviews == null)
        {
            _logger.LogError("[ReviewController] review lsit not found while executing GetAll()");
            return NotFound();
        }
        return View(reviews);
    }
    

    // Actions to create a review
    [HttpGet]
    public IActionResult CreateReview() 
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(Review review)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _reviewRepository.Create(review);
            if(returnOk)
            {
                int productId = review.ProductId;
                _logger.LogInformation("[ReviewController] Review created successfully for ProductId {ProductId:0000}", review.ProductId);
                return RedirectToAction("Details", "Product", new {id = productId});
            }
            else
            {
                _logger.LogError("[ReviewController] Failed to create review for ProductId {ProductId:0000}", review.ProductId);
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

    [HttpPost]
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

        var success = await _reviewRepository.Delete(id);
        if (success)
        {
            _logger.LogInformation("[ReviewController] Successfully deleted review with ReviewId {ReviewId}", id);
            return RedirectToAction("Details", "Product", new { id = review.ProductId });
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
            _logger.LogInformation("[ReviewController] Successfully added review response to review with ReviewId {ReviewId}", review.ReviewId);
            return RedirectToAction("Index");
        }
        else
        {
            _logger.LogError("[ReviewController] Failed to add response to review with ReviewId {ReviewId:0000}", id);
            return BadRequest("Failed to add response");
        }

    }
}