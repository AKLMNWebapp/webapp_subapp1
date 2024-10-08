using System;
namespace mvc.Models;

public enum Role
{
    User, 
    Business,
    Admin
}
public class User {
    public int UserId {get; set;}
    public string Email {get; set;} = string.Empty;
    public Role UserRole {get; set;}
    public string Password {get; set;} = string.Empty;
}