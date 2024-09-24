using System;
namespace mvc.Models;

public class product
 {
    public int PID {get; set;}
    public int CatID {get; set;}
    public int UID {get; set;}
    public String name {get; set;} = String.Empty;
    public int AllergyCode {get; set;}
    public String? description {get; set;}
    public String? imageURL {get; set;}
    public String nutrition {get; set;} = String.Empty;
    public String createdAt {get; set;} = String.Empty;
    public String updatedAt {get; set;} = String.Empty;
 }