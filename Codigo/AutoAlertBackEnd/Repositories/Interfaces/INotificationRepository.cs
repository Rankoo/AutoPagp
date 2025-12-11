using AutoAlertBackEnd.Models;

namespace AutoAlertBackEnd.Repositories;

public interface INotificationRepository
{
    Task<IEnumerable<Notifications>> GetAllAsync();
    Task<Notifications?> GetByIdAsync(Guid id);
    Task<IEnumerable<Notifications>> GetByAlertIdAsync(Guid alertId);
    Task<IEnumerable<Notifications>> GetByUserIdAsync(Guid userId);
    Task<Notifications> CreateAsync(Notifications notification);
    Task<Notifications?> UpdateAsync(Notifications notification);
    Task<bool> DeleteAsync(Guid id);
}

