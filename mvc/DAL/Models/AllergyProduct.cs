using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
namespace mvc.DAL.Models;

public class AllergyProduct
{
    public int AllergyProductId {get; set;}
    public int ProductId {get; set;}

    public int AllergyCode {get; set;}
    [ForeignKey(nameof(AllergyCode))]
    
    public virtual Allergy Allergy {get; set;} = default!;
    public virtual Product Product {get; set;} = default!;
}