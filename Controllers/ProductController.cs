public class ProductsController : Controller
{
    // Action method to handle category details
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
