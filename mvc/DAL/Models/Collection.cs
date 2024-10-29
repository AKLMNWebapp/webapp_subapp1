using System;
using System.ComponentModel.DataAnnotations;
namespace mvc.Models;

public class Collection {
    public int CollectionId {get; set;}

    [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The name must be numbers of letters between 2 to 20 characters.")]
    [Display(Name = "Collection name")]
    public string Name {get; set;} = default!;
    public string UserId 