using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;

namespace mvc.DAL.Repositories;

public class ProductRepository : IRepository<Product>
{
    private readonly ProductDbContext _db;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ProductDbContext db, ILogger<ProductRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        try
        {
            return await _db.Products.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] items ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return new List<Product>();
        }
    }

    public async Task<IEnumerable<Product>> GetAllByUserId(string userId)
    {
        try
        {
            return await _db.Products
            .Where(r => r.UserId == userId)
            .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] ToListAsync() failed when GetAllByUserId() for UserId: {userId}, error message: {e}", e.Message);
            return new List<Product>();
        }
    }

    public async Task<Product?> GetById(int id)
    {
        try
        {
            return await _db.Products.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] items FindAsync() failed when GetById() for ProductId {ProductId:0000}, error message: {e}", 
            id ,e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Product product)
    {
        try
        {
            _db.Products.Add(product); 
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] product creation failed for item {@item}, error message: {e}", product, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Product product)
    {
        try
        {
            _db.Products.Update(product); 
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] product FindAsync(id) failed when updating the ProductId {ProductId}, error message: {e}",
            product, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogError("[ProductRepository] product not found for the ProductId {ProductId:0000}", id);
                return false;
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] product deletion failed for the ProductId {ProductId:0000}, error message: {e}", id, e.Message );
            return false;
        }
    }
}