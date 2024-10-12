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

        if(!context.Allergies.Any())
        {
            var allergies = new List<Allergy>
            {
                new Allergy { AllergyCode = 1, Name = "Peanut"},
                new Allergy { AllergyCode = 2, Name = "Milk"},
                new Allergy { AllergyCode = 3, Name = "Egg" },
                new Allergy { AllergyCode = 4, Name = "Soy"},
            };
            context.Allergies.AddRangeAsync(allergies);
            context.SaveChangesAsync();
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
                new Product
                {
                    Name = "Milk",
                    Energy = 60,
                    Fat = 6,
                    Carbohydrates = 4.5,
                    Protein = 8,
                    Description = "this is milkkkkk",
                    ImageUrl = "/images/milk.jpg",
                    UserId = 1,
                    CategoryId = 1,
                }
            };
            context.AddRange(products);
            context.SaveChanges();
        }

        if(!context.AllergyProducts.Any())
        {
            var allergyProducts = new List<AllergyProduct>
            {
                new AllergyProduct {AllergyCode = 3, ProductId = 1},
                new AllergyProduct {AllergyCode = 2, ProductId = 4}
            };
            context.AllergyProducts.AddRange(allergyProducts);
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
                new Review
                {
                    UserId = 1,
                    ProductId = 1,
                    Rating = 2.5m,
                    Comment = "Hmmmmm",
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
                UserId = 1,
                Name = "Vegan",
                Products = new List<Product>(),
                CreatedAt = DateTime.Now
                },
                new Collection
                {
                UserId = 1,
                Name = "Meat",
                Products = new List<Product>(),
                CreatedAt = DateTime.Now
                },
            };
            context.Collections.AddRange(collection);
            context.SaveChanges();
        }
    }
}
