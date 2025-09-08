using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OData.Models;
using OData.Models.Interfaces;

namespace OData.Controllers;

[Route("odata/[controller]")]
public class ProductsController : ODataController
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [EnableQuery]
    public IQueryable<Product> Get()
    {
        return _productService.GetProducts();
    }

    [EnableQuery]
    public ActionResult<Product> Get(int key)
    {
        var product = _productService.GetProduct(key);
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    public async Task<IActionResult> Post([FromBody] Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var created = await _productService.CreateProduct(product);
        return Created(created);
    }

    public async Task<IActionResult> Put(int key, [FromBody] Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var updated = await _productService.UpdateProduct(key, product);
        return Updated(updated);
    }

    public async Task<IActionResult> Delete(int key)
    {
        await _productService.DeleteProduct(key);
        return NoContent();
    }
}