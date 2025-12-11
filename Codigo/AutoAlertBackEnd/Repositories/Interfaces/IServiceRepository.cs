using AutoAlertBackEnd.Models;

namespace AutoAlertBackEnd.Repositories;

public interface IServiceRepository
{
    Task<IEnumerable<Services>> GetAllAsync();
    Task<Services?> GetByIdAsync(Guid id);
    Task<IEnumerable<Services>> GetByStoreIdAsync(Guid storeId);
    Task<Services> CreateAsync(Services service);
    Task<Services?> UpdateAsync(Services service);
    Task<bool> DeleteAsync(Guid id);
}

