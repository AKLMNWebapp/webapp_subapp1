using System;
using System.ComponentModel.DataAnnotations;
namespace mvc.DAL.Models;

public class Review
{
    public int ReviewId {get; set;}
    public string UserId {get; set;} = string.Empty;
    public virtual ApplicationUser? User {get; set; } //navigation property
    public int ProductId {get; set;}
    public virtual Product? Product {get; set;} //navigation property

   [StringLength(500)]
    public string? Comment {get; set;}
    public DateTime? CreatedAt {get; set;} = DateTime.Now;

    [StringLength(500)]
    public string? Response {get; set;}

    public string? ResponseUserID {get; set;}

    public virtual ApplicationUser? ResponseUser {get; set;} // navigation property
}