using AutoAlertBackEnd.Models;

namespace AutoAlertBackEnd.Repositories;

public interface IStoreRepository
{
    Task<IEnumerable<Stores>> GetAllAsync();
    Task<Stores?> GetByIdAsync(Guid id);
    Task<Stores> CreateAsync(Stores store);
    Task<Stores?> UpdateAsync(Stores store);
    Task<bool> DeleteAsync(Guid id);
}
