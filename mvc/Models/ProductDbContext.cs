using Microsoft.EntityFrameworkCore;

namespace mvc.Models;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Allergy> Allergies { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Collection> Collections { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<User> Users  {get; set; }
    public virtual DbSet<AllergyProduct> AllergyProducts {get; set;}
    public virtual DbSet<ProductCategory> ProductCategories {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}