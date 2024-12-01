using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Moq;
using mvc.Controllers;
using mvc.DAL.Models;
using mvc.DAL.ViewModels;
using mvc.DAL.Repositories;
using mvc.DAL;


namespace mvc.Test.Controllers;


public class ProductControllerTests
{
    private readonly Mock<IRepository<Product>> mockProductRepository;
    private readonly Mock<IRepository<Allergy>> mockAllergyRepository;
    private readonly Mock<ILogger<ProductController>> mockLogger;
    private readonly Mock<IRepository<Review>> mockReviewRepository;
    private readonly Mock<IRepository<Category>> mockCategoryRepository;
    private readonly Mock<UserManager<ApplicationUser>> mockUserManager;

    public ProductControllerTests()
    {
        mockProductRepository = new Mock<IRepository<Product>>();
        mockAllergyRepository = new Mock<IRepository<Allergy>>();
        mockLogger = new Mock<ILogger<ProductController>>();
        mockReviewRepository = new Mock<IRepository<Review>>();
        mockCategoryRepository = new Mock<IRepository<Category>>();

        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

        mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );
    }

    //Tests the Index (list) functionaltity which displays a lists of products
    [Fact]
    public async Task ListProducts()
    {
    // arrange
        var products = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                Name = "TestProduct",
                Energy = 100,
                Fat = 3,
                Carbohydrates = 20,
                Protein = 10,
                Description = "A test product"
            }
        };

        mockProductRepository.Setup(repo => repo.GetAll()).ReturnsAsync(products);

        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object, 
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object);

    // act
    var result = await productController.Index();


    // assert
    var viewResult = Assert.IsType<ViewResult>(result);
    var productViewModel = Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);
    Assert.Single(productViewModel.Products);
    }

    //Test list functionality unseccessful
    [Fact]
    public async Task ListProductsNegative()
    {
        //arrange

        //returning an empty list
        mockProductRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Product>());
        
        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object, 
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object);

        //act
        var result = await productController.Index();
        
        //assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<ProductViewModel>(viewResult.Model);
        Assert.Empty(viewModel.Products);
    }


    //Tests behvaior of CreateProduct when it fails
    [Fact]
    public async Task CreateProductFailed()
    {
        //arrange
        var testProduct = new Product
        {
            ProductId = 1,
            Name = "TestProduct",
            Energy = 100,
            Fat = 3,
            Carbohydrates = 20,
            Protein = 10,
            Description = "A test product"
        };

        var createProductViewModel = new CreateProductViewModel
        {
            Product = testProduct,
            SelectedAllergyCodes = new List<int> { 1, 2 }
        };

        mockProductRepository.Setup(repo => repo.Create(testProduct)).ReturnsAsync(false);
        mockAllergyRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((int id) =>
            new Allergy { AllergyCode = id, Name = $"Allergy{id}" });        
        
        mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser { Id = "1", UserName = "TestUser" });

        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object, 
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object
        )
        {
            // Override the User.FindFirstValue logic
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        productController.ControllerContext.HttpContext.Items["MockedUserId"] = "1";

        //act
        var result = await productController.CreateProduct(createProductViewModel);


        //assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Product creation failed", badRequestResult.Value);
    }

    //Tests creation of a product w/ allergies
    [Fact]
    public async Task CreateProductwithAllergies()
    {
        //arrange
        var testProd = new Product 
        { 
            ProductId = 1, 
            Name = "TestProd", 
            AllergyProducts = new List<AllergyProduct>()
        };

        var allergies = new List<Allergy>
        {
            new Allergy { AllergyCode = 1, Name = "Peanuts" },
            new Allergy { AllergyCode = 2, Name = "Milk" },
            new Allergy { AllergyCode = 3, Name = "Eggs" },
            new Allergy { AllergyCode = 4, Name = "Gluten" }
        };


        var createProductViewModel = new CreateProductViewModel
        {
            Product = testProd,
            SelectedAllergyCodes = new List<int> {1, 2, 3, 4}
        };

        mockAllergyRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
            .ReturnsAsync((int id) => allergies.FirstOrDefault(a => a.AllergyCode == id));
        
        mockProductRepository.Setup(repo => repo.Create(It.IsAny<Product>())).ReturnsAsync(true);

        mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser { Id = "1", UserName = "TestUser" });        
        
        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object, 
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() 
            }
        };

        productController.ControllerContext.HttpContext.Items["MockedUserId"] = "1";

        //act
        var result = await productController.CreateProduct(createProductViewModel);

        //assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);

        //verif
        Assert.Equal(4, testProd.AllergyProducts.Count); 
    }

    [Fact]
    public async Task UpdateProduct()
    {
        //arrange
        var productUpdate = new Product
        {
            ProductId = 1,
            Name = "TestProductUpdateeee",
            Energy = 200,
            Fat = 6,
            Carbohydrates = 20,
            Protein = 10,
            Description = "A test product updatedddd!!!",
            AllergyProducts = new List<AllergyProduct>()
        };

        var createProductViewModel = new CreateProductViewModel
        {
            Product = productUpdate,
            SelectedAllergyCodes = new List<int>(),
            AllergyMultiSelectList = new List<SelectListItem>(),
            NewAllergyName = string.Empty
        };

        mockProductRepository.Setup(repo => repo.Update(productUpdate)).ReturnsAsync(true);

        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object, 
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object);

        //act
        var result = await productController.Update(createProductViewModel);

        //assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    //Test update product fails
    [Fact]
    public async Task UpdateProductFail()
    {
        //arrange
        var productUpdateInvalid = new Product
        {
            ProductId = 1,
            Name = "TestProductUpdateeee",
            Energy = 200,
            Fat = 6,
            Carbohydrates = 20,
            Protein = 10,
            Description = "A test product updatedddd fail????!!!",
            AllergyProducts = new List<AllergyProduct>()
        };

        var createProductViewModel = new CreateProductViewModel
        {
            Product = productUpdateInvalid,
            SelectedAllergyCodes = new List<int>(),
            AllergyMultiSelectList = new List<SelectListItem>(),
            NewAllergyName = string.Empty
        };

        mockProductRepository.Setup(repo => repo.Update(productUpdateInvalid)).ReturnsAsync(false);

        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object, 
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object);

        //act
        var result = await productController.Update(createProductViewModel);

        //assert
        if (result is ViewResult viewResult)
        {
            // check if the ViewResult is returned due to ModelState errors
            Assert.IsType<ViewResult>(result);
            Assert.Equal(createProductViewModel, viewResult.Model);
        }
        else if (result is BadRequestObjectResult badRequestResult)
        {
            // check if BadRequestObjectResult is returned due to repository failure
            Assert.Equal("Product creation failed", badRequestResult.Value);
        }
        else
        {
            // fail the test if neither result is returned
            Assert.Fail("Unexpected result type");
        }
    }

    //Tests the Delete GET method
    [Fact]
    public async Task DeleteProduct()
    {
        //arrange
        var testProduct = new Product { ProductId = 1, Name = "TestProduct"};

        mockProductRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(testProduct);

        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object, 
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object);

        //act
        var result = await productController.Delete(1);

        //assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Product>(viewResult.Model);
        Assert.Equal(testProduct, model);
    }

    //Tests the DeleteConfirmed POST method
    [Fact]
    public async Task DeleteProductConfirmed()
    {
        //arrange
        //simulating a successful deletion
        mockProductRepository.Setup(repo => repo.Delete(1)).ReturnsAsync(true);

        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object, 
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object);

        //act
        var result = await productController.DeleteConfirmed(1);

        //assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);

        //verify
        mockProductRepository.Verify(repo => repo.Delete(1), Times.Once);
    }

    [Fact]
    public async Task DeleteProductFailed()
    {
        //arrange
        int invalidProductId = -3;
        mockProductRepository.Setup(repo => repo.Delete(invalidProductId)).ReturnsAsync(false);

        var productController = new ProductController(
            mockProductRepository.Object, 
            mockAllergyRepository.Object,
            mockCategoryRepository.Object,
            mockReviewRepository.Object,
            mockUserManager.Object,
            mockLogger.Object);

        //act
        var result = await productController.DeleteConfirmed(invalidProductId);

        //assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Product not found for the ProductId", badRequestResult.Value);

        //verify only attempted once
        mockProductRepository.Verify(repo => repo.Delete(invalidProductId), Times.Once);
    }
}
