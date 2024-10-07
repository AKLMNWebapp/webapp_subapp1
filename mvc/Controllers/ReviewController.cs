
using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace mvc.Controllers;

public class ReviewController : Controller
{
    private readonly ProductDbContext _context;

    public ReviewController (ProductDbContext context)
    {
        _context = context;
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
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            int productId = review.ProductId;
            return RedirectToAction("Details", "Product", new {id = productId});
        }

        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }
        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        int productId = review.ProductId; //get ProductId associated with the review, for redirect after deletion.

        _context.Reviews.Remove(review); //removes the review from the db
        await _context.SaveChangesAsync(); //coomits changes to db
        return RedirectToAction("Details", "Product", new {id = productId}); //redirects to the Details page (spørs hva vi gjør) for the product associated with the deleted review.
    }
}