using AutoAlertBackEnd.Context;
using AutoAlertBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAlertBackEnd.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly AutoAlertContext _context;

    public AlertRepository(AutoAlertContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Alerts>> GetAllAsync()
    {
        return await _context.Alerts.ToListAsync();
    }

    public async Task<Alerts?> GetByIdAsync(Guid id)
    {
        return await _context.Alerts.FindAsync(id);
    }

    public async Task<IEnumerable<Alerts>> GetByServiceIdAsync(Guid serviceId)
    {
        return await _context.Alerts
            .Where(a => a.ServiceId == serviceId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Alerts>> GetScheduledAlertsAsync(DateTime? fromDate = null)
    {
        var query = _context.Alerts.AsQueryable();
        
        if (fromDate.HasValue)
        {
            query = query.Where(a => a.ScheduledAt >= fromDate.Value);
        }
        
        return await query.OrderBy(a => a.ScheduledAt).ToListAsync();
    }

    public async Task<Alerts> CreateAsync(Alerts alert)
    {
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<Alerts?> UpdateAsync(Alerts alert)
    {
        var existing = await _context.Alerts.FindAsync(alert.Id);
        if (existing == null) return null;
        _context.Entry(existing).CurrentValues.SetValues(alert);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Alerts.FindAsync(id);
        if (existing == null) return false;
        _context.Alerts.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}

