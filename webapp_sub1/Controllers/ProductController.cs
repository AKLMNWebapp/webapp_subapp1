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

            return View(subcategories);
        }

        // Action method to display the "Vegan Products" category with subcategories
        public IActionResult VeganProducts()
        {
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
                }
            };

            return View(subcategories);
        }

        // Action method to display the "Dairy, Cheese, and Eggs" category with subcategories
        public IActionResult DairyCheeseEggs()
        {
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
                }
            };

            return View(subcategories);
        }

        // Action method to display the "Beverages" category with subcategories
        public IActionResult Beverages()
        {
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
                }
            };

            return View(subcategories);
        }

        // Handle category details for other categories
        public IActionResult CategoryDetails(string category)
        {
            var products = GetProductsByCategory(category);
            ViewData["Category"] = category;
            return View(products);
        }

        // Mock method for retrieving products by category (replace with actual data access logic)
        private List<Product> GetProductsByCategory(string category)
        {
            return new List<Product>
            {
                new Product { Name = "Sample Product 1", ImageUrl = "sample1.jpg" },
                new Product { Name = "Sample Product 2", ImageUrl = "sample2.jpg" }
            };
        }
    }
}
