using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class SaleItemsService
{
    private readonly ISaleItemRepository _saleItemRepository;

    public SaleItemsService(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }
}
