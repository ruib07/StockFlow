using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface IPurchaseRepository
{
    Task<IEnumerable<Purchases>> GetPurchases();
    Task<Purchases> GetPurchaseById(Guid purchaseId);
    Task<Purchases> CreatePurchase(Purchases purchase);
    Task UpdatePurchase(Purchases purchase);
    Task DeletePurchase(Guid purchaseId);
}
