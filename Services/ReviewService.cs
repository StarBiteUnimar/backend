using rest_rating.Models;
using rest_rating.Repositories;

namespace rest_rating.Services;

public class ReviewService
{
    private readonly IReviewRepository _reviews;
    private readonly IRestaurantRepository _restaurants;
    private readonly IUserRepository _users;

    public ReviewService(IReviewRepository reviews, IRestaurantRepository restaurants, IUserRepository users)
    {
        _reviews = reviews;
        _restaurants = restaurants;
        _users = users;
    }

    public async Task<Review> CreateReviewAsync(Review review)
    {
        if (review.Stars < 4)
            throw new InvalidOperationException("Only reviews with 4 or 5 stars can be submitted.");

        await _reviews.AddAsync(review);
        await _reviews.SaveChangesAsync();

        return review;
    }

    public async Task ApproveReviewAsync(Guid restaurantId, Guid reviewId)
    {
        var rev = await _reviews.GetByIdAsync(reviewId);
        if (rev == null) throw new KeyNotFoundException("Review not found");
        if (rev.RestaurantId != restaurantId) throw new UnauthorizedAccessException();

        if (rev.Status != ReviewStatus.Pending) throw new InvalidOperationException("Review already processed");

        var rest = await _restaurants.GetByIdAsync(restaurantId);
        if (rest == null) throw new KeyNotFoundException("Restaurant not found");

        if (rest.DailyReviewCount >= 10) throw new InvalidOperationException("Daily review limit reached");

        rev.Status = ReviewStatus.Approved;
        rev.ApprovedAt = DateTime.UtcNow;
        rest.DailyReviewCount++;
        await _reviews.SaveChangesAsync();

        var approved = await _reviews.ListApprovedByRestaurantAsync(restaurantId);
        if (approved.Any())
        {
            rest.RatingAverage = approved.Average(r => r.Stars);
            await _restaurants.SaveChangesAsync();
        }

        var user = await _users.GetByIdAsync(rev.UserId);
        if (user != null)
        {
            user.Points += 10;
            await _users.SaveChangesAsync();
        }
    }
}
