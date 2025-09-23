using rest_rating.Models;

namespace rest_rating.Repositories;

public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdAsync(Guid id);
    Task AddAsync(Restaurant restaurant);
    Task SaveChangesAsync();
    Task<IEnumerable<Restaurant>> ListAsync();
}
