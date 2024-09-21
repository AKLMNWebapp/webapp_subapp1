using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace mvc.Controllers;

public class ProductController : Controller
{
    private readonly ProductDbContext _productDbContext;

    public ProductController(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }
}