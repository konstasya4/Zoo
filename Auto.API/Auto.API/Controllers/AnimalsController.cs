using Auto.API.Models;
using Auto.Core.Entities;
using Auto.Data;
using Microsoft.AspNetCore.Mvc;

namespace Auto.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly IAutoStorage _db;

    public AnimalsController(IAutoStorage db)
    {
        this._db = db;
    }
   const int PAGE_SIZE = 5;

    // GET: api/animals
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListAnimals().Skip(index).Take(count)
            .Select(v => v.ToResource());
        var total = _db.CountAnimals();
        var _links = HAL.PaginateAsDynamic("/api/animals", index, count, total);
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
        var categoryAnimal = _db.FindAnimal(id);
        if (categoryAnimal == default) return NotFound();
        var resource = categoryAnimal.ToDynamic();
        resource._actions = new
        {
            create = new
            {
                href = $"/api/animals/{id}",
                type = "application/json",
                method = "POST",
                name = $"Create a new {categoryAnimal.Title}"
            }
        };
        return Ok(resource);
    }
    // PUT api/animals/{id}
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] AnimalDto dto)
    {
        var animal = new Animal
        {
            Title = dto.Title,
            Code = dto.Code,
        };
        _db.UpdateAnimal(animal);
        return Ok(dto);
    }
    // POST api/animals
    [HttpPost("{id}")]
    public async Task<IActionResult> Post(string id, [FromBody] AnimalDto dto)
    {
        var existing = _db.FindAnimal(dto.Code);
        if (existing != default)
            return Conflict($"Sorry, there is already a product with serial {dto.Code} in the database.");
        var animal = new Animal
        {
            Title = dto.Title,
            Code = dto.Code,
        };
        _db.CreateAnimal(animal);
        //await PublishNewAnimalMessage(animal);
        return Created($"/api/animals/{animal.Code}", animal.ToResource());
    }
    // DELETE api/animals/ABC123
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var animal = _db.FindAnimal(id);
        if (animal == default) return NotFound();
        _db.DeleteAnimal(animal);
        return NoContent();
    }

}