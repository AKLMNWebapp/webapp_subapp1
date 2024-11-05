using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using webapp_sub1.Models;

namespace webapp_sub1.Controllers
{
    public class ProductsController : Controller
    {
        // Action method to display the "Fruits and Vegetables" category with subcategories
        public IActionResult FruitsAndVegetables()
        {
            // Mock data for subcategories and products
            var subcategories = new List<Subcategory>
            {
                new Subcategory
                {
                    Name = "Citrus Fruits",
                    Products = new List<Product>
                    {
                        new Product { Name = "Orange", ImageUrl = "orange.jpg" },
                        new Product { Name = "Lemon", ImageUrl = "lemon.jpg" },
                        new Product { Name = "Lime", ImageUrl = "lime.jpg" },
                        new Product { Name = "Grapefruit", ImageUrl = "grapefruit.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Leafy Greens",
                    Products = new List<Product>
                    {
                        new Product { Name = "Spinach", ImageUrl = "spinach.jpg" },
                        new Product { Name = "Kale", ImageUrl = "kale.jpg" },
                        new Product { Name = "Lettuce", ImageUrl = "lettuce.jpg" },
                        new Product { Name = "Cabbage", ImageUrl = "cabbage.jpg" }
                    }
                }
            };

            return View(subcategories);  // Pass the subcategories to the view
        }

        // Action method to display the "Vegan Products" category with subcategories
        public IActionResult VeganProducts()
        {
            // Mock data for vegan subcategories and products
            var subcategories = new List<Subcategory>
            {
                new Subcategory
                {
                    Name = "Plant-Based Milk Alternatives",
                    Products = new List<Product>
                    {
                        new Product { Name = "Almond Milk", ImageUrl = "almond_milk.jpg" },
                        new Product { Name = "Soy Milk", ImageUrl = "soy_milk.jpg" },
                        new Product { Name = "Oat Milk", ImageUrl = "oat_milk.jpg" },
                        new Product { Name = "Coconut Milk", ImageUrl = "coconut_milk.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Vegan Cheese",
                    Products = new List<Product>
                    {
                        new Product { Name = "Cashew Cheese", ImageUrl = "cashew_cheese.jpg" },
                        new Product { Name = "Soy Cheese", ImageUrl = "soy_cheese.jpg" },
                        new Product { Name = "Almond Cheese", ImageUrl = "almond_cheese.jpg" },
                        new Product { Name = "Coconut Cheese", ImageUrl = "coconut_cheese.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Meat Substitutes",
                    Products = new List<Product>
                    {
                        new Product { Name = "Tofu", ImageUrl = "tofu.jpg" },
                        new Product { Name = "Tempeh", ImageUrl = "tempeh.jpg" },
                        new Product { Name = "Seitan", ImageUrl = "seitan.jpg" },
                        new Product { Name = "Lentil Burgers", ImageUrl = "lentil_burgers.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Vegan Snacks",
                    Products = new List<Product>
                    {
                        new Product { Name = "Vegan Chips", ImageUrl = "vegan_chips.jpg" },
                        new Product { Name = "Fruit Snacks", ImageUrl = "fruit_snacks.jpg" },
                        new Product { Name = "Nut Bars", ImageUrl = "nut_bars.jpg" },
                        new Product { Name = "Veggie Sticks", ImageUrl = "veggie_sticks.jpg" }
                    }
                }
            };

            return View(subcategories); // Pass the subcategories to the view
        }

        // Action method to display the "Dairy, Cheese, and Eggs" category with subcategories
        public IActionResult DairyCheeseEggs()
        {
            // Mock data for dairy subcategories and products
            var subcategories = new List<Subcategory>
            {
                new Subcategory
                {
                    Name = "Milk",
                    Products = new List<Product>
                    {
                        new Product { Name = "Whole Milk", ImageUrl = "whole_milk.jpg" },
                        new Product { Name = "Almond Milk", ImageUrl = "almond_milk.jpg" },
                        new Product { Name = "Soy Milk", ImageUrl = "soy_milk.jpg" },
                        new Product { Name = "Oat Milk", ImageUrl = "oat_milk.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Cheese",
                    Products = new List<Product>
                    {
                        new Product { Name = "Cheddar", ImageUrl = "cheddar.jpg" },
                        new Product { Name = "Mozzarella", ImageUrl = "mozzarella.jpg" },
                        new Product { Name = "Feta", ImageUrl = "feta.jpg" },
                        new Product { Name = "Vegan Cheese", ImageUrl = "vegan_cheese.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Yogurt",
                    Products = new List<Product>
                    {
                        new Product { Name = "Greek Yogurt", ImageUrl = "greek_yogurt.jpg" },
                        new Product { Name = "Flavored Yogurt", ImageUrl = "flavored_yogurt.jpg" },
                        new Product { Name = "Dairy-Free Yogurt", ImageUrl = "dairy_free_yogurt.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Eggs",
                    Products = new List<Product>
                    {
                        new Product { Name = "Organic Eggs", ImageUrl = "organic_eggs.jpg" },
                        new Product { Name = "Free-Range Eggs", ImageUrl = "free_range_eggs.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Cream",
                    Products = new List<Product>
                    {
                        new Product { Name = "Heavy Cream", ImageUrl = "heavy_cream.jpg" },
                        new Product { Name = "Sour Cream", ImageUrl = "sour_cream.jpg" },
                        new Product { Name = "Cream Cheese", ImageUrl = "cream_cheese.jpg" }
                    }
                }
            };

            return View(subcategories); // Pass the subcategories to the view
        }

        // Action method to display the "Beverages" category with subcategories
        public IActionResult Beverages()
        {
            // Mock data for beverage subcategories and products
            var subcategories = new List<Subcategory>
            {
                new Subcategory
                {
                    Name = "Juices",
                    Products = new List<Product>
                    {
                        new Product { Name = "Orange Juice", ImageUrl = "orange_juice.jpg" },
                        new Product { Name = "Apple Juice", ImageUrl = "apple_juice.jpg" },
                        new Product { Name = "Grape Juice", ImageUrl = "grape_juice.jpg" },
                        new Product { Name = "Pineapple Juice", ImageUrl = "pineapple_juice.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Soda",
                    Products = new List<Product>
                    {
                        new Product { Name = "Coca Cola", ImageUrl = "coca_cola.jpg" },
                        new Product { Name = "Pepsi", ImageUrl = "pepsi.jpg" },
                        new Product { Name = "Sprite", ImageUrl = "sprite.jpg" },
                        new Product { Name = "Fanta", ImageUrl = "fanta.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Water",
                    Products = new List<Product>
                    {
                        new Product { Name = "Mineral Water", ImageUrl = "mineral_water.jpg" },
                        new Product { Name = "Sparkling Water", ImageUrl = "sparkling_water.jpg" },
                        new Product { Name = "Flavored Water", ImageUrl = "flavored_water.jpg" }
                    }
                },
                new Subcategory
                {
                    Name = "Smoothies",
                    Products = new List<Product>
                    {
                        new Product { Name = "Strawberry Banana Smoothie", ImageUrl = "strawberry_banana_smoothie.jpg" },
                        new Product { Name = "Mango Smoothie", ImageUrl = "mango_smoothie.jpg" },
                        new Product { Name = "Green Smoothie", ImageUrl = "green_smoothie.jpg" },
                        new Product { Name = "Blueberry Smoothie", ImageUrl = "blueberry_smoothie.jpg" }
                    }
                }
            };

            return View(subcategories);  // Pass the subcategories to the view
        }

        // Existing method: Handle category details for other categories
        public IActionResult CategoryDetails(string category)
        {
            var products = GetProductsByCategory(category);

            ViewData["Category"] = category;
            return View(products);
        }

        // Mock method for retrieving products (replace this with your actual data access logic)
        private List<Product> GetProductsByCategory(string category)
        {
            return new List<Product>
            {
                new Product { Name = "Product 1", ImageUrl = "product1.jpg" },
                new Product { Name = "Product 2", ImageUrl = "product2.jpg" }
            };
        }
    }
}
