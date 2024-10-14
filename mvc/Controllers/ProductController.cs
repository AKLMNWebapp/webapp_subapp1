using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.ViewModels;
using mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using mvc.DAL;

namespace mvc.Controllers;

public class ProductController : Controller
{
    private readonly IRepository _productRepository;

    public ProductController(IRepository productRepository)
    {
        _productRepository = productRepository; //initialize the db
    }

    public async Task<IActionResult> Index()
    {
        //retrieve all products from db
        var products = await _productRepository.GetAll(); 
        //return the view with list of products
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
            return BadRequest("Product not found.");
        return View(product);
    }

    
    // This Get request populates the Allergy section with already existing allergies in the database
    [HttpGet]
    public async Task<IActionResult> CreateProduct() 
    {
        var allergies = await _productRepository.GetAll(); // gets list of all available allergies

        // Our viewModel here is used to list all allergies in our select menu on the view
        var createProductViewModel = new CreateProductViewModel
        {
            Product = new Product(),
            AllergyMultiSelectList = allergies.Select(allergy => new SelectListItem {
                Value = allergy.AllergyCode.ToString(),
                Text = allergy.Name
            }).ToList()
        };

        return View(createProductViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
    {
        var product = model.Product;
        if (ModelState.IsValid)
        {

            // This code ensures that the product can only be made by users, that are logged in
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userID))
            {
                return Forbid();
            }

            // This code handles user with self-made allergies that are not already pre existing in out database
            product.UserId = int.Parse(userID); // parses id to string to use in if statement
            if (!string.IsNullOrEmpty(model.NewAllergyName)) {
                var newAllergy = new Allergy {Name = model.NewAllergyName}; // Creates a new allergy
                _productRepository.Allergies.Add(newAllergy);
                await _productRepository.SaveChangesAsync(); // Saves new allergy to database
                model.SelectedAllergyCodes.Add(newAllergy.AllergyCode); // adds the new allergies codes to viewModel
            }

            // This loop iterates through all allergy codes that have been selected from the allergy menu
            // The selected allergies will be saved as an AllergyProduct
            foreach (var allergyCode in model.SelectedAllergyCodes)
            {
                product.AllergyProducts.Add(new AllergyProduct {
                    AllergyCode = allergyCode,
                    Product = product
                });
            }

            _productRepository.Create(product);
            await _productRepository.SaveChangesAsync(); // updated the db with new product
            return RedirectToAction("Index"); // redirects to Index view.
        }
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id) {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CreateProductViewModel model) 
    {
        var product = model.Product;
        if (ModelState.IsValid)
        {

            // This code ensures that the product can only be made by users, that are logged in
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userID))
            {
                return Forbid();
            }

            // This code handles user with self-made allergies that are not already pre existing in out database
            product.UserId = int.Parse(userID); // parses id to string to use in if statement
            if (!string.IsNullOrEmpty(model.NewAllergyName)) {
                var newAllergy = new Allergy {Name = model.NewAllergyName}; // Creates a new allergy
                _productRepository.Allergies.Add(newAllergy);
                await _productRepository.SaveChangesAsync(); // Saves new allergy to database
                model.SelectedAllergyCodes.Add(newAllergy.AllergyCode); // adds the new allergies codes to viewModel
            }

            // This loop iterates through all allergy codes that have been selected from the allergy menu
            // The selected allergies will be saved as an AllergyProduct
            foreach (var allergyCode in model.SelectedAllergyCodes)
            {
                product.AllergyProducts.Add(new AllergyProduct {
                    AllergyCode = allergyCode,
                    Product = product
                });
            }

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(); // updated the db with new product
            return RedirectToAction("Index"); // redirects to Index view.
        }
        return View(product);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}