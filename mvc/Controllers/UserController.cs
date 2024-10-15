using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.DAL;

namespace mvc.Controllers;

public class UserController : Controller
{
    private readonly ProductDbContext _context;

    public UserController(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.ToListAsync(); //retrieve users from db
        return View(users); //returns view with list of users
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user) {
        if(ModelState.IsValid) 
        {
            _context.Users.Add(user); // User is added to db
            await _context.SaveChangesAsync(); // changes are saved to db
            return RedirectToAction(nameof(Index)); // redirects to listing view with updated list
        }
        return View(user);
    }

    // view for updating a specific user
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // Updates user from id specified in GetRequest
    [HttpPost]
    public async Task<IActionResult> Update(User user)
    {
        if(