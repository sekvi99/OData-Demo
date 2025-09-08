using Microsoft.EntityFrameworkCore;
using OData.Data;
using OData.Models;
using OData.Models.Interfaces;

namespace OData.Services;

public class ProductService : IProductService
{
    private readonly ProductContext _context;
    
    public ProductService(ProductContext context)
    {
        _context = context;
    }
    
    public IQueryable<Product> GetProducts()
    {
        return _context.Products.Include(p => p.Category);
    }
    
    public Product? GetProduct(int id)
    {
        return _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
    }
    
    public async Task<Product> CreateProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
    
    public async Task<Product> UpdateProduct(int id, Product product)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing != null)
        {
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.CategoryId = product.CategoryId;
            await _context.SaveChangesAsync();
        }
        return product;
    }
    
    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<string> ResetPrices()
    {
        var products = _context.Products.ToList();
        foreach (var product in products)
        {
            product.Price = 0;
        }
        await _context.SaveChangesAsync();
        return "All prices have been reset to 0";
    }
}
