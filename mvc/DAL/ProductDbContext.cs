using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;

namespace mvc.DAL;

public class ProductDbContext : IdentityDbContext<ApplicationUser>
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Allergy> Allergies { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Collection> Collections { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<ApplicationUser> AppUsers  {get; set; }
    public virtual DbSet<AllergyProduct> AllergyProducts {get; set;}
    //public virtual DbSet<ProductCategory> ProductCategories {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}