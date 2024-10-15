using Microsoft.EntityFrameworkCore;
using mvc.DAL;
using mvc.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProductDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ProductDbContextConnection"]);
});

builder.Services.AddScoped<IRepository<Product>, ProductRepository>();

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information() // Levels: Trace< Information < Warning < Error < Fatal
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd:HHmmss}.log");

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Excecuted DbCommand"));

var logger = loggerConfiguration.C