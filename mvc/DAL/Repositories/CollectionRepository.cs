using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;

namespace mvc.DAL.Repositories;

public class CollectionRepository : IRepository<Collection>
{
    private readonly ProductDbContext _db;
    private readonly ILogger<CollectionRepository> _logger;

    public CollectionRepository(ProductDbContext db, ILogger<CollectionRepository> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<IEnumerable<Collection>> GetAll()
    {
        try
        {
            return await _db.Collections.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[CollectionRepository] ToListAsync() failed when GetAll(), error message: {e}",e.Message);
            return new List<Collection>();
        }
    }

    public async Task<IEnumerable<Collection>> GetAllByUserId(string userId)
    {
        try
        {
            return await _db.Collections
            .Where(r => r.UserId == userId)
            .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[CollectionRepository] ToListAsync() failed when GetAllByUserId() for UserId: {userId}, error message: {e}", e.Message);
            return new List<Collection>();
        }
    }

    public async Task<Collection?> GetById(int id)
    {
        try
        {
            return await _db.Collections.FirstOrDefaultAsync (c => c.CollectionId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[CollectionRepository] GetById failed, error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Collection collection)
    {
        try
        {
            _db.Collections.Add(collection);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CollectionRepository] collection creation failed for collection {@collection}, error message: {e}", collection, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Collection collection)
    {
        try
        {
            _db.Collections.Update(collection);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CollectionRepository] FindAsync(id) failed when updating the CollectionId {CollectionId:0000}, error message: {e}", collection, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var collection = await _db.Collections.FindAsync(id);
            if (collection == null )
            {
                _logger.LogError("[CollectionRepository] collection not found for the CollectionId {CollectionId:0000}", id);
                return false;
            }
            _db.Collections.Remove(collection);
            await _db.SaveChangesAsync();
            return true;    
        }
        catch (Exception e)
        {
            _logger.LogError("[CollectionRepository] collection deletion failed for the CollectionId {CollectionId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}