using Microsoft.AspNetCore.Mvc;
using rest_rating.Repositories;
using rest_rating.Models;

namespace rest_rating.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;
    public UsersController(IUserRepository users) => _users = users;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var u = await _users.GetByIdAsync(id);
        if (u == null) return NotFound();
        return Ok(new { u.Id, u.Email, u.Name, u.PhotoUrl, u.Location, u.Bio, u.Points });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
    {
        var u = await _users.GetByIdAsync(id);
        if (u == null) return NotFound();
        u.Name = dto.Name ?? u.Name;
        u.PhotoUrl = dto.PhotoUrl ?? u.PhotoUrl;
        u.Location = dto.Location ?? u.Location;
        u.Bio = dto.Bio ?? u.Bio;
        await _users.SaveChangesAsync();
        return NoContent();
    }
}

public record UpdateUserDto(string? Name, string? PhotoUrl, string? Location, string? Bio);
