using AutoAlertBackEnd.Context;
using AutoAlertBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAlertBackEnd.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly AutoAlertContext _context;

    public StoreRepository(AutoAlertContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Stores>> GetAllAsync()
    {
        return await _context.Stores.ToListAsync();
    }

    public async Task<Stores?> GetByIdAsync(Guid id)
    {
        return await _context.Stores.FindAsync(id);
    }

    public async Task<Stores> CreateAsync(Stores store)
    {
        _context.Stores.Add(store);
        await _context.SaveChangesAsync();
        return store;
    }

    public async Task<Stores?> UpdateAsync(Stores store)
    {
        var existing = await _context.Stores.FindAsync(store.Id);
        if (existing == null) return null;
        _context.Entry(existing).CurrentValues.SetValues(store);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Stores.FindAsync(id);
        if (existing == null) return false;
        _context.Stores.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
