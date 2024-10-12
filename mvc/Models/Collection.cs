using System;
namespace mvc.Models;

public class Collection {
    public int CollectionId {get; set;}
    public string Name {get; set;} = default!;
    public int UserId {get; set;}
    public virtual User User { get; set; } = default!; //navigation property
    public virtual List<Product> Products {get; set; } = new List<Product>(); //navigation property
    public DateTime? CreatedAt {get; set;}
}