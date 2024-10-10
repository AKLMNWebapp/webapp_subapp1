using System;
namespace mvc.Models;

public class Category 

{
    public int CategoryId {get; set;}
    public string Name {get; set;} = string.Empty;
    //public virtual List<ProductCategory> ProductCategories {get; set;} = default!;
    public virtual List<Product> Products {get; set;} = new List<Product>();

}