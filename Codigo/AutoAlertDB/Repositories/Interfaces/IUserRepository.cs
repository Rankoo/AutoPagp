using AutoAlertDB.Models;

namespace AutoAlertDB.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<Users>> GetAllUsersAsync();
    Task<Users?> GetUserByIdAsync(Guid id);
    Task<Users> CreateUserAsync(Users user);
    Task<Users?> UpdateUserAsync(Users user);
    Task<bool> DeleteUserAsync(Guid id);
    Task<Users?> GetUserByEmailAsync(string email);
}