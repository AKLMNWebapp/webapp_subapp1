using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.DAL;

public class UserRepository : IRepository<User>
{
    private readonly ProductDbContext _db;
    private ILogger<UserRepository> _logger;

    public UserRepository(ProductDbContext db, ILogger<UserRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        try
        {
            return await _db.Users.ToArrayAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[UserRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return new List<User>();
        }
    }

    public async Task<User?> GetById(int id)
    {
        try
        {
            return await _db.Users.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[UserRepository] user FindAsync(id) failed when GetById for UserId {UserId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(User user)
    {
        try
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[UserRepository] user creation failed for user {@user}, error message: {e}", user, e.Message);
            return false;
        }  
    }

    public async Task<bool> Update(User user)
    {
        try
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[UserRepository] user FindAsync(id) failed when updating the UserId {UserId:0000}, error message: {e}", user, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogError("[UserRepository] user not found for the UserId {UserId:0000}", id);
                return false;
            }
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[UserRepository] user deletion failed for the UserId {UserId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }



}