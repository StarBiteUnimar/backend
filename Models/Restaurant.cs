using System.ComponentModel.DataAnnotations;

namespace rest_rating.Models;

public class Restaurant
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Guid? OwnerUserId { get; set; }
    public int DailyReviewCount { get; set; } = 0;
    public double RatingAverage { get; set; } = 0.0;
}
