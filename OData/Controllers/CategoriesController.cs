using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using OData.Data;
using OData.Models;

namespace OData.Controllers;

[Route("odata/[controller]")]
public class CategoriesController : ODataController
{
    private readonly ProductContext _context;
    
    public CategoriesController(ProductContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public IQueryable<Category> Get()
    {
        return _context.Categories.Include(c => c.Products);
    }

    [EnableQuery]
    public ActionResult<Category> Get(int key)
    {
        var category = _context.Categories.Include(c => c.Products)
            .FirstOrDefault(c => c.Id == key);
        if (category == null)
            return NotFound();
        return Ok(category);
    }
}