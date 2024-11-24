using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;


namespace mvc.DAL;

public static class DBInit
{

    // Seeding roles
    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = {"Admin", "Business", "User"};
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    // Seeding Users
    private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager) 
    {
        var adminEmail = "admin@example.com";
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true   
        };

        var businessEmail = "business1@example.com";
        var businessUser = new ApplicationUser
        {
            UserName = businessEmail,
            Email = businessEmail,
            EmailConfirmed = true 
        }; 

        var regularUserEmail = "user@example.com";
        var regularUser = new ApplicationUser
        {
            UserName = regularUserEmail,
            Email = regularUserEmail,
            EmailConfirmed = true  
        };         

        if (await userManager.FindByEmailAsync(adminEmail) == null) {
            await userManager.CreateAsync(adminUser, "Password123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        if (await userManager.FindByEmailAsync(businessEmail) == null) {
            await userManager.CreateAsync(businessUser, "Password123!");
            await userManager.AddToRoleAsync(businessUser, "Business");
        }

        if (await userManager.FindByEmailAsync(regularUserEmail) == null) {
            await userManager.CreateAsync(regularUser, "Password123!");
            await userManager.AddToRoleAsync(regularUser, "User");
        }
    }

    // Seeding data
    public static async Task Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ProductDbContext context = serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>();

        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.EnsureDeleted(); 
        context.Database.EnsureCreated();

        // seed roles and admin user
        await SeedRolesAsync(roleManager);
        await SeedUserAsync(userManager);

        var adminUser = await userManager.FindByEmailAsync("admin@example.com");
        var businessUser = await userManager.FindByEmailAsync("business1@example.com");
        var user = await userManager.FindByEmailAsync("user@example.com");

        if (adminUser == null || businessUser == null || user == null)
        {
            throw new InvalidOperationException("One or more required users are missing");
        }

        // This method assures our different user roles are created when the application starts.

         if(!context.Allergies.Any())
         {
             var allergies = new List<Allergy>
             {
                 new Allergy { AllergyCode = 1, Name = "Peanut"},
                 new Allergy { AllergyCode = 2, Name = "Milk"},
                 new Allergy { AllergyCode = 3, Name = "Egg" },
                 new Allergy { AllergyCode = 4, Name = "Soy"},
             };
             await context.Allergies.AddRangeAsync(allergies);
             await context.SaveChangesAsync();
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
             await context.Categories.AddRangeAsync(categories);
             await context.SaveChangesAsync();
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
                     UserId = businessUser.Id,
                     CategoryId = 1,
                     AllergyProducts = new List<AllergyProduct>
                     {
                        new AllergyProduct
                        {
                            AllergyCode = 3,
                            Allergy = await context.Allergies.FirstOrDefaultAsync(a => a.AllergyCode == 3)
                            ?? throw new InvalidOperationException("Allergy with code 3 not found"),
                        }
                     }
                 },
                 new Product
                 {
                     Name = "Banana",
                     Energy = 2.0,
                     Fat = 1.2,
                     Carbohydrates = 3.2,
                     Protein = 2.3,
                     Description = "test",
                     ImageUrl = "/images/bananas.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 4,
                 },
                 new Product
                 {
                     Name = "Asparagues",
                     Energy = 2.0,
                     Fat = 1.2,
                     Carbohydrates = 3.2,
                     Protein = 2.3,
                     Description = "test",
                     ImageUrl = "/images/asparagus.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 4,
                 },
                 new Product
                 {
                     Name = "Cheese",
                     Energy = 2.0,
                     Fat = 1.2,
                     Carbohydrates = 3.2,
                     Protein = 2.3,
                     Description = "test",
                     ImageUrl = "/images/cheese.jpg",
                     UserId = businessUser.Id,
                     CategoryId = 1,
                     AllergyProducts = new List<AllergyProduct>
                     {
                        new AllergyProduct
                        {
                            AllergyCode = 2,
                            Allergy = await context.Allergies.FirstOrDefaultAsync(a => a.AllergyCode == 2)
                            ?? throw new InvalidOperationException("Allergy with code 2 not found"),
                        }
                     }
                 },
                 new Product
                 {
                     Name = "Vegan Cheese",
                     Energy = 2.0,
                     Fat = 1.2,
                     Carbohydrates = 3.2,
                     Protein = 2.3,
                     Description = "test",
                     ImageUrl = "/images/vegan_cheese.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 2,
                 },
                 new Product
                 {
                     Name = "Meat",
                     Energy = 2.0,
                     Fat = 1.2,
                     Carbohydrates = 3.2,
                     Protein = 2.3,
                     Description = "test",
                     ImageUrl = "/images/meat.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 3,
                 },
                 new Product
                 {
                     Name = "Chicken Breast",
                     Energy = 2.0,
                     Fat = 1.2,
                     Carbohydrates = 3.2,
                     Protein = 2.3,
                     Description = "test",
                     ImageUrl = "/images/chickenbreast.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 3,
                 },

                 new Product
                 {
                     Name = "Oatmilk",
                     Energy = 3,
                     Fat = 3,
                     Carbohydrates = 2.3,
                     Protein = 2,
                     Description = "test",
                     ImageUrl = "/images/oatmilk.jpeg",
                     UserId = businessUser.Id,
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
                     ImageUrl = "/images/apple.jpeg",
                     UserId = businessUser.Id,
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
                     UserId = businessUser.Id,
                     CategoryId = 1,
                     AllergyProducts = new List<AllergyProduct>
                     {
                        new AllergyProduct
                        {
                            AllergyCode = 2,
                            Allergy = await context.Allergies.FirstOrDefaultAsync(a => a.AllergyCode == 2)
                            ?? throw new InvalidOperationException("Allergy with code 2 not found")
                        },
                        new AllergyProduct
                        {
                            AllergyCode = 3,
                            Allergy = await context.Allergies.FirstOrDefaultAsync(a => a.AllergyCode == 3)
                            ?? throw new InvalidOperationException("Allergy with code 3 not found")
                        }
                        //new AllergyProduct {AllergyCode = 2, Allergy = await context.Allergies.FirstOrDefaultAsync(a => a.AllergyCode == 2)},
                        //new AllergyProduct {AllergyCode = 3, Allergy = await context.Allergies.FirstOrDefaultAsync(a => a.AllergyCode == 3)}
                     }
                 }
             };
             await context.AddRangeAsync(products);
             await context.SaveChangesAsync();
         }

         if(!context.AllergyProducts.Any())
         {
             var allergyProducts = new List<AllergyProduct>
             {
                 new AllergyProduct {AllergyCode = 3, ProductId = 1},
                 new AllergyProduct {AllergyCode = 2, ProductId = 4}
             };
             await context.AllergyProducts.AddRangeAsync(allergyProducts);
             await context.SaveChangesAsync();
         }

         if(!context.Reviews.Any())
         {
             var review = new List<Review>
             {
                 new Review
                 {
                     UserId = user.Id,
                     ProductId = 1,
                     Comment = "Amazing",
                     CreatedAt = DateTime.UtcNow
                 },
                 new Review
                 {
                     UserId = user.Id,
                     ProductId = 1,
                     Comment = "Hmmmmm",
                     CreatedAt = DateTime.UtcNow
                 },
             };
             await context.AddRangeAsync(review);
            await context.SaveChangesAsync();
         }

         if(!context.Collections.Any())
         {
             var collection = new List<Collection>
             {
                 new Collection
                 {
                 UserId = user.Id,
                 Name = "Vegan",
                 Products = new List<Product>(),
                 CreatedAt = DateTime.Now
                 },
                 new Collection
                 {
                 UserId = user.Id,
                 Name = "Meat",
                 Products = new List<Product>(),
                 CreatedAt = DateTime.Now
                 },
                 new Collection
                 {
                 UserId = businessUser.Id,
                 Name = "Chickennn",
                 Products = new List<Product>(),
                 CreatedAt = DateTime.Now
                 },
                 
             };
             await context.Collections.AddRangeAsync(collection);
             await context.SaveChangesAsync();
         }
    }
    
}
