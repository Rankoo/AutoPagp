using AutoAlertBackEnd.Context;
using AutoAlertBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAlertBackEnd.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AutoAlertContext _context;

    public ServiceRepository(AutoAlertContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Services>> GetAllAsync()
    {
        return await _context.Services.ToListAsync();
    }

    public async Task<Services?> GetByIdAsync(Guid id)
    {
        return await _context.Services.FindAsync(id);
    }

    public async Task<IEnumerable<Services>> GetByStoreIdAsync(Guid storeId)
    {
        return await _context.Services
            .Where(s => s.StoreId == storeId)
            .ToListAsync();
    }

    public async Task<Services> CreateAsync(Services service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task<Services?> UpdateAsync(Services service)
    {
        var existing = await _context.Services.FindAsync(service.Id);
        if (existing == null) return null;
        _context.Entry(existing).CurrentValues.SetValues(service);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Services.FindAsync(id);
        if (existing == null) return false;
        _context.Services.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}

