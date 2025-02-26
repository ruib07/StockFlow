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

    public Task<IEnumerable<SaleItems>> GetSaleItemsBySaleId(Guid saleId)
    {
        throw new NotImplementedException();
    }

    public Task<SaleItems> CreateSaleItem(SaleItems item)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSaleItem(SaleItems item)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSaleItem(Guid saleItemId)
    {
        throw new NotImplementedException();
    }
}
