using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class SaleItemRepository : ISaleItemRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<SaleItems> SaleItems => _context.SaleItems;

    public SaleItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SaleItems> GetSaleItemById(Guid saleItemId)
    {
        return await SaleItems.FirstOrDefaultAsync(si => si.Id == saleItemId);
    }

    public async Task<IEnumerable<SaleItems>> GetSaleItemsBySaleId(Guid saleId)
    {
        return await SaleItems.AsNoTracking().Where(si => si.SaleId == saleId).ToListAsync();
    }

    public async Task<SaleItems> CreateSaleItem(SaleItems saleItem)
    {
        await SaleItems.AddAsync(saleItem);
        await _context.SaveChangesAsync();

        return saleItem;
    }

    public async Task UpdateSaleItem(SaleItems saleItem)
    {
        SaleItems.Update(saleItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSaleItem(Guid saleItemId)
    {
        var saleItem = await GetSaleItemById(saleItemId);

        SaleItems.Remove(saleItem);
        await _context.SaveChangesAsync();
    }
}
