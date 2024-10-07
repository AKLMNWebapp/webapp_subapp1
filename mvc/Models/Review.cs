using System;
namespace mvc.Models;

public class Review
 {
    public int ReviewId {get; set;}
    public int UserId {get; set;}
    public virtual User User {get; set; } = default!; //navigation property
    public int ProductId {get; set;}
    public virtual Product Product {get; set;} = default!; //navigation property
    public decimal Rating {get; set;}
    public string? Comment {get; set;}
    public DateTime? CreatedAt {get; set;}
 }