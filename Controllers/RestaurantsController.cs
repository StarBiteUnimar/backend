using Microsoft.AspNetCore.Mvc;
using rest_rating.Models;
using rest_rating.Repositories;

namespace rest_rating.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantRepository _restaurants;
    public RestaurantsController(IRestaurantRepository restaurants) => _restaurants = restaurants;

    [HttpPost]
    public async Task<IActionResult> Create(CreateRestaurantDto dto)
    {
        var r = new Restaurant { Name = dto.Name, Address = dto.Address };
        await _restaurants.AddAsync(r);
        await _restaurants.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = r.Id }, r);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var r = await _restaurants.GetByIdAsync(id);
        if (r == null) return NotFound();
        return Ok(r);
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var list = await _restaurants.ListAsync();
        return Ok(list);
    }
}

public record CreateRestaurantDto(string Name, string? Address);
