using Microsoft.AspNetCore.Identity;
using mvc.DAL.Models;

namespace mvc.DAL.ViewModels;

public class BusinessProductViewModel
{
    public IEnumerable<Product> Products;
    public ApplicationUser? User;
    public string? CurrentViewName;

    public BusinessProductViewModel(IEnumerable<Product> products, string? currentViewName, ApplicationUser? user)
    {
        Products = products;
        CurrentViewName = currentViewName;
        User = user;
    }
}
        

        
    
