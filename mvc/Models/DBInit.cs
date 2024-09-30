using Microsoft.EntityFrameworkCore;

namespace mvc.Models;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ProductDbContext context = serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>();
        context.Database.EnsureDeleted(); 
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User {Email = "test@gmail.com", Password = "test123"},
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        if(!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category {Name = "Dairy"},
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Eggs",
                    Energy = 2.0,
                    Fat = 1.2,
                    Carbohydrates = 3.2,
                    Protein = 2.3,
                    Description = "test",
                    ImageUrl = "/images/eggs.jpg",
                    UserId = 1,
                    CategoryId = 1,
                },
            };
            context.AddRange(products);
            context.SaveChanges();
        }
    }
}
