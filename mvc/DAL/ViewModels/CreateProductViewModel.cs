using Microsoft.AspNetCore.Mvc.Rendering;

using mvc.DAL.Models;

namespace mvc.DAL.ViewModels;

public class CreateProductViewModel
{
    public Product Product {get; set;} = default!;
    public List<SelectListItem> CategorySelectList {get; set;} = new List<SelectListItem>();
    public List<SelectListItem> AllergyMultiSelectList {get; set;} = new List<SelectListItem>();
    public List<int> SelectedAllergyCodes {get; set;} = new List<int>();

}