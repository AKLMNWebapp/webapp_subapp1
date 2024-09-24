using System;
namespace mvc.Models;

public class User {
    public int UID {get; set;}
    public String email {get; set;} = String.Empty;
    public String password {get; set;} = String.Empty;
}