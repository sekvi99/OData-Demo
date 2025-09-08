using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.EntityFrameworkCore;
using OData.Data;
using OData.Models;
using OData.Models.Interfaces;
using OData.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

// Add services
builder.Services.AddScoped<IProductService, ProductService>();

// Build EDM model for OData
static IEdmModel GetEdmModel()
{
    var modelBuilder = new ODataConventionModelBuilder();
    modelBuilder.EntitySet<Product>("Products");
    modelBuilder.EntitySet<Category>("Categories");
    
    // Add custom function
    modelBuilder.Function("GetExpensiveProducts")
        .ReturnsCollectionFromEntitySet<Product>("Products");
    
    // Add custom action
    modelBuilder.Action("ResetPrices")
        .Returns<string>();
    
    return modelBuilder.GetEdmModel();
}

// Add OData services
builder.Services.AddControllers()
    .AddOData(options => options
        .Select()           // Enable $select
        .Filter()           // Enable $filter
        .OrderBy()          // Enable $orderby
        .Expand()           // Enable $expand
        .Count()            // Enable $count
        .SetMaxTop(100)     // Limit top results
        .AddRouteComponents("odata", GetEdmModel()));

// Add CORS for testing
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
    SeedData(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors();
app.UseRouting();
app.MapControllers();

app.Run();

static void SeedData(ProductContext context)
{
    if (context.Categories.Any()) return;

    var electronics = new Category { Id = 1, Name = "Electronics" };
    var books = new Category { Id = 2, Name = "Books" };
    
    context.Categories.AddRange(electronics, books);
    
    context.Products.AddRange(
        new Product { Id = 1, Name = "Laptop", Price = 999.99m, CategoryId = 1 },
        new Product { Id = 2, Name = "Smartphone", Price = 599.99m, CategoryId = 1 },
        new Product { Id = 3, Name = "Tablet", Price = 399.99m, CategoryId = 1 },
        new Product { Id = 4, Name = "C# Programming", Price = 49.99m, CategoryId = 2 },
        new Product { Id = 5, Name = "ASP.NET Core Guide", Price = 59.99m, CategoryId = 2 }
    );
    
    context.SaveChanges();
}