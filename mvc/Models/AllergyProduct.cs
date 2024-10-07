using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
namespace mvc.Models;

public class AllergyProduct
{
    public int id {get; set;}
    public int ProductId {get; set;}
    public int AllergyCode {get; set;}
    
    [ForeignKey(nameof(AllergyCode))]
    [InverseProperty(nameof(Allergy.AllergyProducts))]
    public virtual Allergy Allergy {get; set;} = default!;

    [ForeignKey(nameof(ProductId))]
    [InverseProperty("AllergyProducts")]
    public virtual Product Product {get; set;} = default!;
}