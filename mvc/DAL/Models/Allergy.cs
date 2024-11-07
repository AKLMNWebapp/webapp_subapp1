using System;
using System.ComponentModel.DataAnnotations; // data annotations
namespace mvc.DAL.Models;

public class Allergy
 {
    [Key]
    public int AllergyCode {get; set;}

   [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The name must be numbers of letters between 2 to 20 characters.")]
   [Display(Name = "Allergy name")]
    public string Name {get; set;} = string.Empty;
    public virtual ICollection<AllergyProduct> AllergyProducts {get; set;} = new List<AllergyProduct>();
 }