using Microsoft.AspNetCore.Http.Features;
using mvc.Models;

namespace mvc.DAL;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product?> GetProductById(int id);
    Task Create(Product product);
    Task Update(Product product);
    Task<bool> Delete(int id);
}