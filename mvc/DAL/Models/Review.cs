using System;
using System.ComponentModel.DataAnnotations;
namespace mvc.Models;

public class Review
 {
    public int ReviewId {get; set;}
    public string UserId {get; set;} = string.Empty;
    public virtual ApplicationUser User {get; set; } = default!; //navigation property
    public int ProductId {get; set;}
    public virtual Product Product {get; set;} = default!; //navigation property

   [Range(0, double.MaxValue, ErrorMessage = "