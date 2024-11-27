using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace mvc.DAL.Models;

public class Product
{
    public int ProductId { get; set; }
        
    [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The name must be numbers of letters between 2 to 20 characters.")]
    [Display(Name = "Product name")]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Energy must be a greater than 0.")]
    public double Energy { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Fat must be a greater than 0.")]
    public double Fat { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Carbohydrates must be a greater than 0.")]
    public double Carbohydrates { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Protein must be a greater than 0.")]
        public double Protein { get; set; }

    [StringLength(500)]
    public string? Description { get; set; } //kan disable nullable i .csproj hvis vi ønsker 
    public string? ImageUrl { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; } //navigation property

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; } //navigation property
    public DateTime? CreatedAt {get; set;} = DateTime.Now;
    public virtual List<AllergyProduct> AllergyProducts {get; set;} = new List<AllergyProduct>(); // navigation property
    public virtual List<Review> Reviews {get; set;} = new List<Review>();
}