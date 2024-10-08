using System;
namespace mvc.Models;

public class ProductCategory
{
    public int PcId {get; set;}
    public int CategoryId {get; set;}
    public virtual Category Category {get; set;} = default!;
    public int ProductId {get; set;}
    public virtual Product Product {get; set;} = default!;

}