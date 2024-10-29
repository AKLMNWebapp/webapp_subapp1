using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace mvc.Models;

public enum Role
{
    User, 
    Business,
    Admin
}
public class User : IdentityUser 
{

    [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", ErrorMessage = "Invalid email use format (example@email.com)")]
    public override string Email {get; set;} = string.Empty;
    public Role UserRole {get; set;}
}