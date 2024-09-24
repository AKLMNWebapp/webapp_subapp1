using System;
namespace mvc.Models;

public class Review
 {
    public int RID {get; set;}
    public int UID {get; set;}
    public int PID {get; set;}
    public decimal rating {get; set;}
    public String? comment {get; set;}
    public String createdAt {get; set;} = String.Empty;
 }