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
        // Placeholder for logic to fetch products from database based on the category
        var products = GetProductsByCategory(category);

        // Pass the products and category information to the view
        ViewData["Category"] = category;
        return View(products); // Ensure you have a corresponding view for category details
    }

    // Mock method for retrieving products (replace this with your actual data access logic)
    private List<Product> GetProductsByCategory(string category)
    {
        // This is just a placeholder, use your actual database fetching logic
        return new List<Product>
        {
            new Product { Name = "Product 1", Category = category },
            new Product { Name = "Product 2", Category = category }
        };
    }
}

/*Explanation:
FruitsAndVegetables: This method is specifically for the Fruits and Vegetables category with subcategories (like Citrus Fruits, Leafy Greens). It passes a list of subcategories, each with its respective products, to the view.

CategoryDetails: This method handles the details for any other category. It fetches products based on the selected category (e.g., Beverages, Dairy, etc.).*/