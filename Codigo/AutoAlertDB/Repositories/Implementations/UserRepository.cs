using AutoAlertDB.Context;
using AutoAlertDB.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAlertDB.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AutoAlertContext _context;

    public UserRepository(AutoAlertContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Users>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Users?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<Users> CreateUserAsync(Users user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Users?> UpdateUserAsync(Users user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        
        if (existingUser == null)
            return null;

        _context.Entry(existingUser).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync();
        
        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Users?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}