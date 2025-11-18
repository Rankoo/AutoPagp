using AutoAlertBackEnd.Dtos;
using AutoAlertBackEnd.Models;

namespace AutoAlertBackEnd.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<Users>> GetAllUsersAsync();
    Task<Users?> GetUserByIdAsync(Guid id);
    Task<Users> CreateUserAsync(CreateUserDto newUser);
    Task<Users?> UpdateUserAsync(Users user);
    Task<bool> DeleteUserAsync(Guid id);
    Task<Users?> GetUserByEmailAsync(string email);
}