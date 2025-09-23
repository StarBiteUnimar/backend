using rest_rating.Data;
using rest_rating.Models;
using Microsoft.EntityFrameworkCore;

namespace rest_rating.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _db;
    public ReviewRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Review review) => await _db.Reviews.AddAsync(review);

    public async Task<Review?> GetByIdAsync(Guid id) => await _db.Reviews.FindAsync(id);

    public async Task<IEnumerable<Review>> ListPendingByRestaurantAsync(Guid restaurantId)
    {
        return await _db.Reviews.Where(r => r.RestaurantId == restaurantId && r.Status == ReviewStatus.Pending).ToListAsync();
    }

    public async Task<IEnumerable<Review>> ListApprovedByRestaurantAsync(Guid restaurantId)
    {
        return await _db.Reviews.Where(r => r.RestaurantId == restaurantId && r.Status == ReviewStatus.Approved).ToListAsync();
    }

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
