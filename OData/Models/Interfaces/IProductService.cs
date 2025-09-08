namespace OData.Models.Interfaces;

public interface IProductService
{
    IQueryable<Product> GetProducts();
    Product? GetProduct(int id);
    Task<Product> CreateProduct(Product product);
    Task<Product> UpdateProduct(int id, Product product);
    Task DeleteProduct(int id);
    Task<string> ResetPrices();
}