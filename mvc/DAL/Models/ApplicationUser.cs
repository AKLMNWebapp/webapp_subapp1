using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace mvc.Models;

public class ApplicationUser : IdentityUser
{
    public int ApplicationUserID {get; set;}
    public string Name {get; set;} = string.Empty;
}