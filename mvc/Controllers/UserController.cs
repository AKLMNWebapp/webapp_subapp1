using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;
using mvc.DAL.Repositories;

namespace mvc.Controllers;

public class ApplicationUserController : Controller
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly ILogger<ApplicationUserController> _logger;

    public ApplicationUserController(IRepository<ApplicationUser> userRepository, ILogger<ApplicationUserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userRepository.GetAll(); //retrieve users from db
        if (users == null)
        {
            _logger.LogError("[UserController] user list not found while executing GetAll()");
            return NotFound("User list not found");
        }
        return View(users); //returns view with list of users
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ApplicationUser user) 
    {
        if(ModelState.IsValid) 
        {
            bool returnOk = await _userRepository.Create(user); // User is added to db
            if (returnOk)
            {
                return RedirectToAction(nameof(Index)); // redirects to listing view with updated list
            }           
        }
        _logger.LogError("[UserController] user creation failed {@user}", user); 
        return View(user);
    }

    // view for updating a specific user
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null) NotFound();
        return View(user);
    }

    // Updates user from id specified in GetRequest
    [HttpPost]
    public async Task<IActionResult> Update(ApplicationUser user)
    {
        if(ModelState.IsValid) 
        {
            var returnOk = await _userRepository.Update(user);
            if (returnOk)
            {
                _logger.LogInformation("[UserController] Successfully updated user with UserId {UserId:0000}", user);
                return RedirectToAction(nameof(Index)); // redirects to list view with updated user

            }
            else
            {
                _logger.LogError("[UserController] failed to update user with UserId {UserId:0000}", user);
            }
        }
        return View(user);
    }

    //confirmation view for deleting a specific user
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userRepository.GetById(id); //finds the user by id
        if (user == null)
        {
            _logger.LogError("[UserController] User not found for UserId {UserId:0000}", id);
            return NotFound(); //404 if not found
        }
        return View(user); //return confirmation view with user details
    }

    //deletes the specified user after confirmation
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmation(int id)
    {
        bool returnOk = await _userRepository.Delete(id); //find the user by id
        if (!returnOk)
        {
            _logger.LogError("[UserController] user deletion failed for UserId {UserId:0000}",id);
            return BadRequest("User not found for the UserId"); //404 if not found
        }
        //_context.Users.Remove(user); //removes from db
        //await _context.SaveChangesAsync(); //saves changes to db
        return RedirectToAction(nameof(Index)); //redirects with updated list
    }
}