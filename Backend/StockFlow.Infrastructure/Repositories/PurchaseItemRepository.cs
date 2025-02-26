using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class PurchaseItemRepository : IPurchaseItemRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<PurchaseItems> PurchaseItems => _context.PurchaseItems;

    public PurchaseItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<PurchaseItems>> GetPurchaseItemsByPurchaseId(Guid purchaseId)
    {
        throw new NotImplementedException();
    }

    public Task<PurchaseItems> CreatePurchaseItem(PurchaseItems purchaseItem)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePurchaseItem(PurchaseItems purchaseItem)
    {
        throw new NotImplementedException();
    }

    public Task DeletePurchaseItem(Guid purchaseItemId)
    {
        throw new NotImplementedException();
    }
}
