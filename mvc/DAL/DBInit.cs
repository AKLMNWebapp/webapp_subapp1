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
                     Energy = 155,
                     Fat = 11,
                     Carbohydrates = 1.1,
                     Protein = 13,
                     Description = "Eggs are a versatile and nutritious food, perfect for breakfast, baking, or cooking."+ 
                                    "Rich in protein, vitamins, and minerals, they’re an excellent choice for a balanced diet."+ 
                                    "Whether boiled, scrambled, fried, or baked, eggs are easy to prepare and a staple in many dishes.",
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
                     Energy = 88,
                     Fat = 0.3,
                     Carbohydrates = 23,
                     Protein = 1.1,
                     Description = "Perfect for a quick snck or addition to meals. Bananas are richin ptassium, fiber, and bitamins,"+ 
                                    "making them a healthy chocie for energy and digestion.",
                     ImageUrl = "/images/bananas.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 4,
                 },
                 new Product
                 {
                     Name = "Asparagues",
                     Energy = 20,
                     Fat = 0.1,
                     Carbohydrates = 3.9,
                     Protein = 2.2,
                     Description = "A crisp and tender vegetable with a fresh, earthy flavor. Known for its long, green stalks,"+ 
                                    "it’s a healthy choice rich in vitamins, fiber, and antioxidants."+ 
                                    "Perfect for roasting, grilling, or steaming, asparagus makes a delicious side dish or a nutritious addition to salads and pastas.",
                     ImageUrl = "/images/asparagus.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 4,
                 },
                 new Product
                 {
                     Name = "Cheese",
                     Energy = 402,
                     Fat = 33,
                     Carbohydrates = 1.3,
                     Protein = 25,
                     Description = "A rich and flavorful dairy product enjoyed in countless dishes around the world."+ 
                                    "From creamy and mild to sharp and aged, cheese comes in a variety of types to suit any taste."+
                                    "Packed with calcium and protein, it’s perfect for snacks, sandwiches, pastas, or simply on its own",
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
                     Energy = 277,
                     Fat = 20.3,
                     Carbohydrates = 24,
                     Protein = 10,
                     Description = "A dairy-free alternative made from plant-based ingredients like nuts, soy, or coconut."+ 
                                    "With its creamy texture and rich flavor, it’s perfect for those following a vegan diet or looking for a lactose-free option."+ 
                                    "Ideal for sandwiches, pizzas, or snacks, vegan cheese offers a tasty and versatile way to enjoy cheese without animal products.",
                     ImageUrl = "/images/vegan_cheese.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 2,
                 },
                 new Product
                 {
                     Name = "Meat",
                     Energy = 143,
                     Fat = 3.5,
                     Carbohydrates = 1.2,
                     Protein = 26,
                     Description = "A rich source of protein and nutrients, offering a variety of cuts and flavors to suit any dish."+ 
                                    "Whether it's beef, pork, lamb, or other types, meat is a versatile ingredient that can be grilled, roasted, stewed, or fried."+ 
                                    "It’s a staple in many cuisines, known for its hearty and satisfying taste.",
                     ImageUrl = "/images/meat.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 3,
                 },
                 new Product
                 {
                     Name = "Chicken",
                     Energy = 164,
                     Fat = 3.6,
                     Carbohydrates = 1,
                     Protein = 31,
                     Description = "A versatile and popular source of protein, enjoyed in cuisines around the world."+ 
                                    "Its mild flavor and tender texture make it perfect for roasting, grilling, frying, or adding to soups and salads."+
                                    "Chicken is a healthy choice, packed with essential nutrients and easy to prepare",
                     ImageUrl = "/images/chickenbreast.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 3,
                 },

                 new Product
                 {
                     Name = "Oatmilk",
                     Energy = 46,
                     Fat = 1.5,
                     Carbohydrates = 6.7,
                     Protein = 1,
                     Description = "A creamy and plant-based alternative to dairy milk, made from oats and water."+ 
                                    "It has a naturally sweet and mild flavor, making it perfect for coffee, smoothies, cereals, or baking."+ 
                                    "Rich in fiber and often fortified with vitamins, oat milk is a nutritious and versatile choice for those seeking a dairy-free option.",
                     ImageUrl = "/images/oatmilk.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 2,
                 },
                 new Product
                 {
                     Name = "Apple",
                     Energy = 73,
                     Fat = 0.24,
                     Carbohydrates = 19.3,
                     Protein = 0.3,
                     Description = "Apples are crisp and juicy fruits with a naturally sweet and refreshing flavor. Available in a variety of colors and tastes,"+ 
                                    "from tart green to sweet red, they are perfect as a snack or for use in baking and cooking. Apples are rich in fiber and vitamins,"+ 
                                    "making them a healthy choice for any time of the day.",
                     ImageUrl = "/images/apple.jpeg",
                     UserId = businessUser.Id,
                     CategoryId = 4,
                 },
                 new Product
                 {
                     Name = "Milk",
                     Energy = 42,
                     Fat = 1,
                     Carbohydrates = 5,
                     Protein = 3.4,
                     Description = "A nutritious and versatile drink, rich in calcium, protein, and essential vitamins."+ 
                                    "It’s a staple in many diets, enjoyed on its own or used in cooking, baking, and beverages. Available in various options like whole,"+ 
                                    "low-fat, or lactose-free, milk is perfect for all ages and lifestyles.",
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
