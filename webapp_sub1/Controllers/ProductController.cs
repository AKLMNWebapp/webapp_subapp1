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
