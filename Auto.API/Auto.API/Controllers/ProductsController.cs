using Auto.API.Models;
using Auto.Core.Entities;
using Auto.Data;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace Auto.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IAutoStorage _db;
    private readonly IBus _bus;

    public ProductsController(IAutoStorage db, IBus bus)
    {
        this._db = db;
        this._bus = bus;
    }

    const int PAGE_SIZE = 5;

    // GET: api/products
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListProducts().Skip(index).Take(count)
            .Select(v => v.ToResource());
        var total = _db.CountProducts();
        var _links = HAL.PaginateAsDynamic("/api/products", index, count, total);
        var result = new
        {
            _links,
            count,
            total,
            index,
            items
        };
        return Ok(result);
    }

    // GET api/products/{id}
    [HttpGet("{id}")]
    [Produces("application/hal+json")]
    public IActionResult Get(string id)
    {
        var product = _db.FindProduct(id);
        if (product == default) return NotFound();
        var resource = product.ToResource();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/products/{id}",
                method = "DELETE",
                name = $"Delete {id} from the database"
            }
        };
        return Ok(resource);
    }

    // PUT api/products/{id}
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] ProductDto dto)
    {
        var productCategory = _db.FindCategory(dto.CategoryCode);
        var product = new Product
        {
            Serial = dto.Serial,
            Title = dto.Title,
            Price = dto.Price,
            CategoryCode = productCategory.Code
        };
        _db.UpdateProduct(product);
        return Ok(dto);
    }

    // POST api/products
    [HttpPost("{id}")]
    public async Task<IActionResult> Post(string id, [FromBody] ProductDto dto)
    {
        var existing = _db.FindProduct(dto.Serial);
        if (existing != default)
            return Conflict($"Sorry, there is already a product with serial {dto.Serial} in the database.");
        var productCategory = _db.FindCategory(dto.CategoryCode);
        var product = new Product
        {
            Serial = dto.Serial,
            Title = dto.Title,
            Price = dto.Price,
            ProductCategory = productCategory
        };
        _db.CreateProduct(product);
        await PublishNewProductMessage(product);
        return Created($"/api/products/{product.Serial}", product.ToResource());
    }

    // DELETE api/products/ABC123
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var product = _db.FindProduct(id);
        if (product == default) return NotFound();
        _db.DeleteProduct(product);
        return NoContent();
    }

    private async Task PublishNewProductMessage(Product product)
    {
        var message = product.ToMessage();
        await _bus.PubSub.PublishAsync(message);
    }
}