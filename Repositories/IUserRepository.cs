using rest_rating.Models;

namespace rest_rating.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();  // 👈 novo
    Task SaveChangesAsync();
}
