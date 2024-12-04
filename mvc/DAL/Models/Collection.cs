using System;
using System.ComponentModel.DataAnnotations;
namespace mvc.DAL.Models;

public class Collection {
    public int CollectionId {get; set;}

    [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The name must be numbers of letters between 2 to 20 characters.")]
    [Display(Name = "Collection name")]
    public string Name {get; set;} = default!;
    public string UserId {get; set;} = string.Empty;
    public virtual ApplicationUser User { get; set; } = default!; //navigation property
    public virtual List<Product> Products {get; set; } = new List<Product>(); //navigation property
    public DateTime? CreatedAt {get; set;}
}