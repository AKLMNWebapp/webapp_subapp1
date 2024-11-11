using System;
using System.ComponentModel.DataAnnotations;

namespace mvc.DAL.Models;

public class Category 

{
    public int CategoryId {get; set;}

    [RegularExpression(@"[a-zA-ZæøåÆØÅ, \-]{2,30}", ErrorMessage = "The name must be numbers of letters between 2 to 30 characters.")]
    [Display(Name = "Category name")]
    public string Name {get; set;} = string.Empty;
    //public virtual List<ProductCategory> ProductCategories {get; set;} = default!;
    public virtual List<Product> Products {get; set;} = new List<Product>();

}