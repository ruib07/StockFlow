using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class PurchaseItemsService
{
    private readonly IPurchaseItemRepository _purchaseItemRepository;

    public PurchaseItemsService(IPurchaseItemRepository purchaseItemRepository)
    {
        _purchaseItemRepository = purchaseItemRepository;
    }

    public async Task<PurchaseItems> GetPurchaseItemById(Guid purchaseItemId)
    {
        var purchaseItem = await _purchaseItemRepository.GetPurchaseItemById(purchaseItemId);

        if (purchaseItem == null) ErrorHelper.ThrowNotFoundException("Purchase item not found!");

        return purchaseItem;
    }

    public async Task<IEnumerable<PurchaseItems>> GetPurchaseItemsByPurchaseId(Guid purchaseId)
    {
        var purchaseItemByPurchase = await _purchaseItemRepository.GetPurchaseItemsByPurchaseId(purchaseId);

        if (purchaseItemByPurchase == null || !purchaseItemByPurchase.Any()) ErrorHelper.ThrowNotFoundException("No purchase items found in this purchase!");

        return purchaseItemByPurchase;
    }

    public async Task<PurchaseItems> CreatePurchaseItem(PurchaseItems purchaseItem)
    {
        return await _purchaseItemRepository.CreatePurchaseItem(purchaseItem);
    }

    public async Task<PurchaseItems> UpdatePurchaseItem(Guid purchaseItemId, PurchaseItems updatePurchaseItem)
    {
        var currentPurchaseItem = await GetPurchaseItemById(purchaseItemId);

        currentPurchaseItem.Quantity = updatePurchaseItem.Quantity;
        currentPurchaseItem.UnitPrice = updatePurchaseItem.UnitPrice;
        currentPurchaseItem.SubTotal = updatePurchaseItem.SubTotal;

        await _purchaseItemRepository.UpdatePurchaseItem(currentPurchaseItem);
        return currentPurchaseItem;
    }

    public async Task DeletePurchaseItem(Guid purchaseItemId)
    {
        await _purchaseItemRepository.DeletePurchaseItem(purchaseItemId);
    }
}
