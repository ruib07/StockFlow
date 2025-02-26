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

    public async Task<IEnumerable<Suppliers>> GetSuppliers()
    {
        return await Suppliers.ToListAsync();
    }

    public async Task<Suppliers> GetSupplierById(Guid supplierId)
    {
        return await Suppliers.FirstOrDefaultAsync(sp => sp.Id == supplierId);
    }

    public async Task<Suppliers> CreateSupplier(Suppliers supplier)
    {
        await Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        return supplier;
    }

    public async Task UpdateSupplier(Suppliers supplier)
    {
        Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSupplier(Guid supplierId)
    {
        var supplier = await GetSupplierById(supplierId);

        Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
    }
}
