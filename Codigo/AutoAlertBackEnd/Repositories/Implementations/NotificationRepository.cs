using AutoAlertBackEnd.Context;
using AutoAlertBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAlertBackEnd.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AutoAlertContext _context;

    public NotificationRepository(AutoAlertContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Notifications>> GetAllAsync()
    {
        return await _context.Notifications.ToListAsync();
    }

    public async Task<Notifications?> GetByIdAsync(Guid id)
    {
        return await _context.Notifications.FindAsync(id);
    }

    public async Task<IEnumerable<Notifications>> GetByAlertIdAsync(Guid alertId)
    {
        return await _context.Notifications
            .Where(n => n.AlertId == alertId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notifications>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .ToListAsync();
    }

    public async Task<Notifications> CreateAsync(Notifications notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<Notifications?> UpdateAsync(Notifications notification)
    {
        var existing = await _context.Notifications.FindAsync(notification.Id);
        if (existing == null) return null;
        _context.Entry(existing).CurrentValues.SetValues(notification);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Notifications.FindAsync(id);
        if (existing == null) return false;
        _context.Notifications.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}

