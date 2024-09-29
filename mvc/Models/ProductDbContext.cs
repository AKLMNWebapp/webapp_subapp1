using Microsoft.EntityFrameworkCore;

namespace mvc.Models;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Allergy> Allergies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users  {get; set; }
}