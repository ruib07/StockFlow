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

    public Task<IEnumerable<Sales>> GetSales()
    {
        throw new NotImplementedException();
    }

    public Task<Sales> GetSaleById(Guid saleId)
    {
        throw new NotImplementedException();
    }

    public Task<Sales> CreateSale(Sales sales)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSale(Sales sales)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSale(Guid saleId)
    {
        throw new NotImplementedException();
    }
}
