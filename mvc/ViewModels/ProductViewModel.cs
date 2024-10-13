using mvc.Models;

namespace mvc.ViewModels
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products;
        public string? CurrentViewName;

        public ProductViewModel(IEnumerable<Product> products, string? currentViewName)
        {
            Products = products;
            CurrentViewName = currentViewName;
        }
    }
}