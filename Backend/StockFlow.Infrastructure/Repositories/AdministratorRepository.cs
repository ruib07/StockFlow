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

    public Task<Administrators> GetAdminById(Guid adminId)
    {
        throw new NotImplementedException();
    }

    public Task<Administrators> GetAdminByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Administrators> CreateAdmin(Administrators admin)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAdmin(Administrators admin)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAdmin(Guid adminId)
    {
        throw new NotImplementedException();
    }
}
