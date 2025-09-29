﻿using rest_rating.Data;
using rest_rating.Models;
using Microsoft.EntityFrameworkCore;

namespace rest_rating.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _db.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync() // 👈 novo
    {
        return await _db.Users.ToListAsync();
    }

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}