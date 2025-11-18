using System;
using AutoAlertBackEnd.Context;
using AutoAlertBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAlertBackEnd.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AutoAlertContext _context;

    public RoleRepository(AutoAlertContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Roles>> GetAllRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Roles?> GetRoleByIdAsync(Guid id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<Roles> CreateRoleAsync(Roles role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<Roles?> UpdateRoleAsync(Roles role)
    {
        var existingRole = await _context.Roles.FindAsync(role.Id);
        
        if (existingRole == null)
            return null;

        _context.Entry(existingRole).CurrentValues.SetValues(role);
        await _context.SaveChangesAsync();
        
        return existingRole;
    }

    public async Task<bool> DeleteRoleAsync(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return false;

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Roles?> GetRoleByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }
}