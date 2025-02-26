using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Sales> Sales => _context.Sales;

    public SaleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sales>> GetSales()
    {
        return await Sales.ToListAsync();
    }

    public async Task<Sales> GetSaleById(Guid saleId)
    {
        return await Sales.FirstOrDefaultAsync(s => s.Id == saleId);
    }

    public async Task<Sales> CreateSale(Sales sale)
    {
        await Sales.AddAsync(sale);
        await _context.SaveChangesAsync();

        return sale;
    }

    public async Task UpdateSale(Sales sale)
    {
        Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSale(Guid saleId)
    {
        var sale = await GetSaleById(saleId);

        Sales.Remove(sale);
        await _context.SaveChangesAsync();
    }
}
