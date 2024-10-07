using System;
using System.ComponentModel.DataAnnotations; // data annotations
namespace mvc.Models;

public class Allergy
 {
    [Key]
    public int AllergyCode {get; set;}
    public string Name {get; set;} = string.Empty;
    public virtual ICollection<AllergyProduct> AllergyProducts {get; set;} = new List<AllergyProduct>();
 }