using Microsoft.AspNetCore.Mvc.Rendering;

using mvc.DAL.Models;

namespace mvc.DAL.ViewModels;

public class UpdateProductViewModel
{
    public Product Product {get; set;} = default!;
    public Allergy? Allergy {get; set;}

}