using Microsoft.AspNetCore.Mvc;
using rest_rating.Models;
using rest_rating.Repositories;
using rest_rating.Services;

namespace rest_rating.Controllers;

[ApiController]
[Route("api/restaurants/{restaurantId:guid}/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly ReviewService _service;
    private readonly IReviewRepository _reviews;

    public ReviewsController(ReviewService service, IReviewRepository reviews)
    {
        _service = service;
        _reviews = reviews;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid restaurantId, CreateReviewDto dto)
    {
        var review = new Review
        {
            UserId = dto.UserId,
            RestaurantId = restaurantId,
            Stars = dto.Stars,
            Text = dto.Text
        };

        try
        {
            var created = await _service.CreateReviewAsync(review);
            return Accepted(new { id = created.Id, status = created.Status.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("pending")]
    public async Task<IActionResult> Pending(Guid restaurantId)
    {
        var list = await _reviews.ListPendingByRestaurantAsync(restaurantId);
        return Ok(list);
    }

    [HttpPost("{reviewId:guid}/approve")]
    public async Task<IActionResult> Approve(Guid restaurantId, Guid reviewId)
    {
        try
        {
            await _service.ApproveReviewAsync(restaurantId, reviewId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public record CreateReviewDto(Guid UserId, int Stars, string? Text);
