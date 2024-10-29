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
public class ApplicationUser 
{
    public int ApplicationUserID {get; set;}
    public string Name {get; set;} = string.Empty;
    public Role ApplicationUserRole {get; set;}
}