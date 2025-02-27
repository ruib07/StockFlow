using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class PurchasesService
{
    private readonly IPurchaseRepository _purchaseRepository;

    public PurchasesService(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<IEnumerable<Purchases>> GetPurchases()
    {
        return await _purchaseRepository.GetPurchases();
    }

    public async Task<Purchases> GetPurchaseById(Guid purchaseId)
    {
        var purchase = await _purchaseRepository.GetPurchaseById(purchaseId);

        if (purchase == null) ErrorHelper.ThrowNotFoundException("Purchase not found!");

        return purchase;
    }

    public async Task<Purchases> CreatePurchase(Purchases purchase)
    {
        return await _purchaseRepository.CreatePurchase(purchase);
    }

    public async Task<Purchases> UpdatePurchase(Guid purchaseId, Purchases updatePurchase)
    {
        var currentPurchase = await GetPurchaseById(purchaseId);

        currentPurchase.PurchaseDate = updatePurchase.PurchaseDate;
        currentPurchase.Total = updatePurchase.Total;

        await _purchaseRepository.UpdatePurchase(currentPurchase);
        return currentPurchase;
    }

    public async Task DeletePurchase(Guid purchaseId)
    {
        await _purchaseRepository.DeletePurchase(purchaseId);
    }
}
