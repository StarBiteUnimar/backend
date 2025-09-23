using rest_rating.Models;

namespace rest_rating.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(Guid id);
    Task AddAsync(Review review);
    Task<IEnumerable<Review>> ListPendingByRestaurantAsync(Guid restaurantId);
    Task SaveChangesAsync();
    Task<IEnumerable<Review>> ListApprovedByRestaurantAsync(Guid restaurantId);
}
