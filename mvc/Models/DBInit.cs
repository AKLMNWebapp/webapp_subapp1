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
                new Category {Name = "Dairy, Cheese, and Eggs"},
                new Category {Name = "Vegan"},
                new Category {Name = "Meat"},
                new Category {Name = "Fruits and Vegetables"},
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

                new Product
                {
                    Name = "Vegan",
                    Energy = 3,
                    Fat = 3,
                    Carbohydrates = 2.3,
                    Protein = 2,
                    Description = "test",
                    ImageUrl = "/imagea/milk.jpg",
                    UserId = 1,
                    CategoryId = 2,
                },
                new Product
                {
                    Name = "Apple",
                    Energy = 3,
                    Fat = 3,
                    Carbohydrates = 2.3,
                    Protein = 2,
                    Description = "test",
                    ImageUrl = "/imagea/milk.jpg",
                    UserId = 1,
                    CategoryId = 4,
                },
            };
            context.AddRange(products);
            context.SaveChanges();
        }

        if(!context.Reviews.Any())
        {
            var review = new List<Review>
            {
                new Review
                {
                    UserId = 1,
                    ProductId = 1,
                    Rating = 4.5m,
                    Comment = "Amazing",
                    CreatedAt = DateTime.UtcNow
                },
            };
            context.AddRange(review);
            context.SaveChanges();
        }

        if(!context.Collections.Any())
        {
            var collection = new List<Collection>
            {
                new Collection
                {
                    /*UserId = 1,
                    ProductId = 1,*/
                },
                new Collection
                {
                    /*UserId = 1,
                    ProductId = 2,*/
                },
            };
            context.Collections.AddRange(collection);
            context.SaveChanges();
        }
    }
}
