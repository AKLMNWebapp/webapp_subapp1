using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.DAL;

public class ProductRepository : IRepository<Product>
{
    private readonly ProductDbContext _db;

    public ProductRepository(ProductDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Product?> GetById(int id)
    {
        return await _db.Products.FindAsync(id);
    }

    public async Task Create(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }
}