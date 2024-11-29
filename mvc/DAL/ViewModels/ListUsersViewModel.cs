using mvc.DAL.Models;

namespace mvc.DAL.ViewModels;

public class ListUsersViewModel
{
    public ApplicationUser? user { get; set; }
    public string? Role { get; set; }
}
