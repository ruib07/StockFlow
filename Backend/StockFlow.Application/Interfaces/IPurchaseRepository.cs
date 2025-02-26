using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface IPurchaseRepository
{
    Task<IEnumerable<Purchases>> GetPurchases();
    Task<Purchases> GetPurchaseById(Guid purchaseId);
    Task<Purchases> CreatePurchase(Purchases purchases);
    Task UpdatePurchase(Purchases purchases);
    Task DeletePurchase(Guid purchaseId);
}
