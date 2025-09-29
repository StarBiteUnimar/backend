using Microsoft.AspNetCore.Mvc;
using rest_rating.Repositories;
using rest_rating.Models;
using System.Security.Cryptography; 

namespace rest_rating.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;

    public UsersController(IUserRepository users) => _users = users;


    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _users.GetAllAsync();
        return Ok(users.Select(u => new {
            u.Id,
            u.Email,
            u.Name,
            u.PhotoUrl,
            u.Location,
            u.Bio,
            u.Points
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var u = await _users.GetByIdAsync(id);
        if (u == null) return NotFound();
        return Ok(new { u.Id, u.Email, u.Name, u.PhotoUrl, u.Location, u.Bio, u.Points });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new { error = "O campo 'password' é obrigatório." });
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            Name = dto.Name,
            PasswordHash = HashPassword(dto.Password),
            PhotoUrl = dto.PhotoUrl,
            Location = dto.Location,
            Bio = dto.Bio,
            Points = 0
        };

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = user.Id }, new
        {
            user.Id,
            user.Email,
            user.Name,
            user.PhotoUrl,
            user.Location,
            user.Bio,
            user.Points
        });
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

public record CreateUserDto(string Email, string Name, string Password, string? PhotoUrl, string? Location, string? Bio);
public record UpdateUserDto(string? Name, string? PhotoUrl, string? Location, string? Bio);
