using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OData.Models;
using OData.Models.Interfaces;

namespace OData.Controllers;

[Route("odata")]
public class FunctionsController : ODataController
{
    private readonly IProductService _productService;
    
    public FunctionsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet("GetExpensiveProducts")]
    [EnableQuery]
    public IQueryable<Product> GetExpensiveProducts()
    {
        return _productService.GetProducts().Where(p => p.Price > 500);
    }
    
    [HttpPost("ResetPrices")]
    public async Task<ActionResult<string>> ResetPrices()
    {
        var result = await _productService.ResetPrices();
        return Ok(result);
    }
}