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
