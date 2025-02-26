using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface IPurchaseItemRepository
{
    Task<PurchaseItems> GetPurchaseItemById(Guid purchaseItemId);
    Task<IEnumerable<PurchaseItems>> GetPurchaseItemsByPurchaseId(Guid purchaseId);
    Task<PurchaseItems> CreatePurchaseItem(PurchaseItems purchaseItem);
    Task UpdatePurchaseItem(PurchaseItems purchaseItem);
    Task DeletePurchaseItem(Guid purchaseItemId);
}
