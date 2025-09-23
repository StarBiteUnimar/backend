using System.ComponentModel.DataAnnotations;

namespace rest_rating.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string PasswordHash { get; set; } = null!;
    public string? Name { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public int Points { get; set; } = 0;
}
