using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Suppliers> Suppliers => _context.Suppliers;

    public SupplierRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Suppliers>> GetSuppliers()
    {
        throw new NotImplementedException();
    }

    public Task<Suppliers> GetSupplierById(Guid supplierId)
    {
        throw new NotImplementedException();
    }

    public Task<Suppliers> CreateSupplier(Suppliers supplier)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSupplier(Suppliers supplier)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSupplier(Guid supplierId)
    {
        throw new NotImplementedException();
    }
}
