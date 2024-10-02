using System;
namespace mvc.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Energy { get; set; }
        public double Fat { get; set; }
        public double Carbohydrates { get; set; }
        public double Protein { get; set; }
        public string? Description { get; set; } //kan disable nullable i .csproj hvis vi ønsker 
        public string? ImageUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = default!; //navigation property
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!; //navigation property

        public ICollection<Review> Reviews {get; set;} = new List<Review>();
    }
}