using System;
namespace mvc.Models;

public class User {
    public int UserId {get; set;}
    public string Email {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public virtual ICollection<Product> Products {get; set;} = new List<Product>();
}