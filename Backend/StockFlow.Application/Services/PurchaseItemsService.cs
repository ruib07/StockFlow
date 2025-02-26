using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class PurchaseItemsService
{
    private readonly IPurchaseItemRepository _purchaseItemRepository;

    public PurchaseItemsService(IPurchaseItemRepository purchaseItemRepository)
    {
        _purchaseItemRepository = purchaseItemRepository;
    }
}
