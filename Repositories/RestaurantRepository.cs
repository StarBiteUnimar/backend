using rest_rating.Data;
using rest_rating.Models;
using Microsoft.EntityFrameworkCore;

namespace rest_rating.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly AppDbContext _db;
    public RestaurantRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Restaurant restaurant)
    {
        await _db.Restaurants.AddAsync(restaurant);
    }

    public async Task<Restaurant?> GetByIdAsync(Guid id) => await _db.Restaurants.FindAsync(id);

    public async Task<IEnumerable<Restaurant>> ListAsync() => await _db.Restaurants.ToListAsync();

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
