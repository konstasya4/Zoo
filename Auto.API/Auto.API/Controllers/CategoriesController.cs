using Auto.API.Models;
using Auto.Core.Entities;
using Auto.Data;
using Microsoft.AspNetCore.Mvc;

namespace Auto.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IAutoStorage _db;

    public CategoriesController(IAutoStorage db)
    {
        this._db = db;
    }
    const int PAGE_SIZE = 5;

    // GET: api/categories
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListCategories().Skip(index).Take(count)
            .Select(v => v.ToResource());
        var total = _db.CountCategories();
        var _links = HAL.PaginateAsDynamic("/api/categories", index, count, total);
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

    [HttpGet("{id}")]
    [Produces("application/hal+json")]

    public IActionResult Get(string id)
    {
        var productCategory = _db.FindCategory(id);
        if (productCategory == default) return NotFound();
        var resource = productCategory.ToDynamic();
        resource._actions = new
        {
            create = new
            {
                href = $"/api/categories/{id}",
                type = "application/json",
                method = "POST",
                name = $"Create a new {productCategory.CategoryAnimal.Title} {productCategory.Title}"
            }
        };
        return Ok(resource);
    }
    // PUT api/categories/{id}
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] CategoryDto dto)
    {
        var categoryAnimal = _db.FindAnimal(dto.AnimalCode);
        var category = new Category
        {
            Title = dto.Title,
            Code = dto.Code,
            AnimalCode = categoryAnimal.Code
        };
        _db.UpdateCategory(category);
        return Ok(dto);
    }
    // POST api/categories
    [HttpPost("{id}")]
    public async Task<IActionResult> Post(string id, [FromBody] CategoryDto dto)
    {
        var existing = _db.FindCategory(dto.Code);
        if (existing != default)
            return Conflict($"Sorry, there is already a product with serial {dto.Code} in the database.");
        var categoryAnimal = _db.FindAnimal(dto.AnimalCode);
        var category = new Category
        {
            Title = dto.Title,
            Code = dto.Code,
            CategoryAnimal = categoryAnimal
        };
        _db.CreateCategory(category);
        //await PublishNewCategoryMessage(category);
        return Created($"/api/categories/{category.Code}", category.ToResource());
    }
    // DELETE api/categories/ABC123
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var category = _db.FindCategory(id);
        if (category == default) return NotFound();
        _db.DeleteCategory(category);
        return NoContent();
    }

}