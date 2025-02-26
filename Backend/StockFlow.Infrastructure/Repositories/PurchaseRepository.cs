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

    public async Task<IEnumerable<Purchases>> GetPurchases()
    {
        return await Purchases.ToListAsync();
    }

    public async Task<Purchases> GetPurchaseById(Guid purchaseId)
    {
        return await Purchases.FirstOrDefaultAsync(p => p.Id == purchaseId);
    }

    public async Task<Purchases> CreatePurchase(Purchases purchase)
    {
        await Purchases.AddAsync(purchase);
        await _context.SaveChangesAsync();

        return purchase;
    }

    public async Task UpdatePurchase(Purchases purchase)
    {
        Purchases.Update(purchase);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePurchase(Guid purchaseId)
    {
        var purchase = await GetPurchaseById(purchaseId);

        Purchases.Remove(purchase);
        await _context.SaveChangesAsync();
    }
}
