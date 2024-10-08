using Microsoft.AspNetCore.Mvc.Rendering;

using mvc.Models;

namespace mvc.ViewModels;

public class CreateProductViewModel
{
    public Product Product {get; set;} = default!;
    public List<SelectListItem> AllergyMultiSelectList {get; set;} = default!;
    public List<int> SelectedAllergyCodes {get; set;} = new List<int>();
    public String NewAllergyName {get; set;} = string.Empty;

}