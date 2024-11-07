using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Define the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add a specific route for vegan products
app.MapControllerRoute(
    name: "veganProducts",
    pattern: "Products/VeganProducts",
    defaults: new { controller = "Products", action = "VeganProducts" });

// Add a specific route for dairy, cheese, and eggs products
app.MapControllerRoute(
    name: "dairyCheeseEggs",
    pattern: "Products/DairyCheeseEggs",
    defaults: new { controller = "Products", action = "DairyCheeseEggs" });

app.Run();

// Add a specific route for Beverages page
app.MapControllerRoute(
    name: "beverages",
    pattern: "Products/Beverages",
    defaults: new { controller = "Products", action = "Beverages" });

app.Run();