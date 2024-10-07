using Microsoft.AspNetCore.Mvc.Rendering;

using mvc.Models;

namespace mvc.ViewModels;

public class CreateProductViewModel
{
    public CreateProductViewModel product {get; set;} = default!;
    public List<MultiSelectList> AllergyMultiSelectList {get; set;} = default!;
    public List<SelectListItem> ReviewSelectList {get; set;} = default!;
}