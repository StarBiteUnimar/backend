using Microsoft.AspNetCore.Mvc;
using rest_rating.Models;
using rest_rating.Repositories;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace rest_rating.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _users;
    public AuthController(IUserRepository users) => _users = users;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existing = await _users.GetByEmailAsync(dto.Email);
        if (existing != null) return BadRequest(new { message = "Email already registered" });

        var hash = HashPassword(dto.Password);
        var user = new User { Email = dto.Email, PasswordHash = hash, Name = dto.Name };
        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return CreatedAtAction(null, new { id = user.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _users.GetByEmailAsync(dto.Email);
        if (user == null) return Unauthorized();

        if (!VerifyPassword(dto.Password, user.PasswordHash)) return Unauthorized();

        return Ok(new { message = "Authenticated (stub)", userId = user.Id });
    }

    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128/8);
        string saltB64 = Convert.ToBase64String(salt);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100000, 32));
        return $"{saltB64}:{hashed}";
    }

    private static bool VerifyPassword(string password, string stored)
    {
        var parts = stored.Split(':');
        if (parts.Length != 2) return false;
        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);
        var attempted = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100000, 32);
        return attempted.SequenceEqual(hash);
    }
}

public record RegisterDto(string Email, string Password, string? Name);
public record LoginDto(string Email, string Password);
