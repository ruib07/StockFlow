using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Administrators> Administrators => _context.Administrators;

    public AdministratorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Administrators> GetAdminById(Guid adminId)
    {
        return await Administrators.FirstOrDefaultAsync(a => a.Id == adminId);
    }

    public async Task<Administrators> GetAdminByEmail(string email)
    {
        return await Administrators.FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<Administrators> CreateAdmin(Administrators admin)
    {
        await Administrators.AddAsync(admin);
        await _context.SaveChangesAsync();

        return admin;
    }

    public async Task UpdateAdmin(Administrators admin)
    {
        Administrators.Update(admin);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAdmin(Guid adminId)
    {
        var admin = await GetAdminById(adminId);

        Administrators.Remove(admin);
        await _context.SaveChangesAsync();
    }
}
