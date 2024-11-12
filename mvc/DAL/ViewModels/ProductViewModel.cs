using mvc.DAL.Models;

namespace mvc.DAL.ViewModels;

public class ProductViewModel
{
    public IEnumerable<Product> Products;
    public IEnumerable<Allergy> Allergies {get; set;}
    public string? CurrentViewName;

    public ProductViewModel(IEnumerable<Product> products, string? currentViewName, IEnumerable<Allergy> allergies)
    {
        Products = products;
        CurrentViewName = currentViewName;
        Allergies = allergies;
    }
}
        

        
    
