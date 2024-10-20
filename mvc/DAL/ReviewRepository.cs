using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.DAL;

public class ReviewRepository : IRepository<Review>
{
    private readonly ProductDbContext _db;
    private readonly ILogger<ReviewRepository> _logger;

    public ReviewRepository(ProductDbContext db, ILogger<ReviewRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Review>> GetAll()
    {
        try
        {
            return await _db.Reviews.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewController] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return new List<Review>();
        }
    }

    public async Task<Review?> GetById(int id)
    {
        try
        {
            return await _db.Reviews.FirstOrDefaultAsync(r => r.ReviewId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] GetById() failed, error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Review review)
    {
        try
        {
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] review creation failed for review {@review}, error message: {e}", review, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Review review)
    {
        try
        {
            _db.Reviews.Update(review);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] Update failed for ReviewId {ReviewId:0000}, error message: {e}", review, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var review = await _db.Reviews.FindAsync(id);
            if(review == null)
            {
                _logger.LogError("[ReviewRepository] review not found for ReviewId {ReviewId:0000}", id);
                return false;
            }
            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();
            return true; 
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] Deletion failed for ReviewId {ReviewId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}