using Microsoft.AspNetCore.Identity;
using mvc.DAL.Models;

namespace mvc.DAL.ViewModels;

public class UserReviewViewModel
{
    public IEnumerable<Review> Reviews;
    public ApplicationUser? User;
    public string? CurrentViewName;

    public UserReviewViewModel(IEnumerable<Review> reviews, string? currentViewName, ApplicationUser? user)
    {
        Reviews = reviews;
        CurrentViewName = currentViewName;
        User = user;
    }
}
        

        
    
