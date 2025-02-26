using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Purchases> Purchases => _context.Purchases;

    public PurchaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Purchases>> GetPurchases()
    {
        throw new NotImplementedException();
    }

    public Task<Purchases> GetPurchaseById(Guid purchaseId)
    {
        throw new NotImplementedException();
    }

    public Task<Purchases> CreatePurchase(Purchases purchases)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePurchase(Purchases purchases)
    {
        throw new NotImplementedException();
    }

    public Task DeletePurchase(Guid purchaseId)
    {
        throw new NotImplementedException();
    }
}
