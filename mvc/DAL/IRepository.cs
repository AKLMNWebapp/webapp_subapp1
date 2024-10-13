using Microsoft.AspNetCore.Http.Features;
using mvc.Models;

namespace mvc.DAL;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    Task Create(T entity);
    Task Update(T entity);
    Task<bool> Delete(int id);
}