using System;
using System.ComponentModel.DataAnnotations;
namespace mvc.Models
{
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

        [StringLength(200)]
