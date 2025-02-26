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

    public async Task<PurchaseItems> GetPurchaseItemById(Guid purchaseItemId)
    {
        return await PurchaseItems.FirstOrDefaultAsync(pi => pi.Id == purchaseItemId);
    }

    public async Task<IEnumerable<PurchaseItems>> GetPurchaseItemsByPurchaseId(Guid purchaseId)
    {
        return await PurchaseItems.AsNoTracking().Where(pi => pi.PurchaseId == purchaseId).ToListAsync();
    }

    public async Task<PurchaseItems> CreatePurchaseItem(PurchaseItems purchaseItem)
    {
        await PurchaseItems.AddAsync(purchaseItem);
        await _context.SaveChangesAsync();

        return purchaseItem;
    }

    public async Task UpdatePurchaseItem(PurchaseItems purchaseItem)
    {
        PurchaseItems.Update(purchaseItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePurchaseItem(Guid purchaseItemId)
    {
        var purchaseItem = await GetPurchaseItemById(purchaseItemId);

        PurchaseItems.Remove(purchaseItem);
        await _context.SaveChangesAsync();
    }
}
