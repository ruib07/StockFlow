using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class PurchasesService
{
    private readonly IPurchaseRepository _purchaseRepository;

    public PurchasesService(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }
}
