using System;
namespace webapp_sub1.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Nutrition { get; set; }
        public string? ImageUrl { get; set; }
        //legger til user og category senere

    }
}
