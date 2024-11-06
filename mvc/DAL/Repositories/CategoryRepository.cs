using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;

namespace mvc.DAL.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly ProductDbContext _db;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(ProductDbContext db, ILogger<CategoryRepository> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<IEnumerable<Category>> GetAll()
    {
        try
        {
            return await _db.Categories.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] categories ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return new List<Category>();
        }
    }

    public async Task<Category?> GetById(int id)
    {
        try
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] FirstOrDefaultAsync() failed when GetById() for CategoryId {CategoryId:0000}, error message: {e}",
            id, e.Message);
            return null;
        }
    }
    public async Task <bool>Create(Category category)
    {
        try
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] category creation failed for categoy {@category}, error message: {e}", category, e.Message);
            return false;
        }
        
    }

    public async Task <bool>Update(Category category)
    {
        try
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] SaveChangesAsync() failed when updating the CategoryId {CategoryId:0000}, error message: {e}",
            category,e.Message);
            return false;
        }
        
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                _logger.LogError("[CateogryRepository] category not found for the CategoryId {CategoryId:0000}", id);
                return false;
            }
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] category deletion failed for CategoryId {CategoryId:0000}, error message: {e}", id, e.Message);
            return false;
        }
        
    }
}