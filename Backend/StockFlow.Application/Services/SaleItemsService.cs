using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class SaleItemsService
{
    private readonly ISaleItemRepository _saleItemRepository;

    public SaleItemsService(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public async Task<SaleItems> GetSaleItemById(Guid saleItemId)
    {
        var saleItem = await _saleItemRepository.GetSaleItemById(saleItemId);

        if (saleItem == null) ErrorHelper.ThrowNotFoundException("Sale item not found!");

        return saleItem;
    }

    public async Task<IEnumerable<SaleItems>> GetSaleItemsBySaleId(Guid saleId)
    {
        var saleItemBySale = await _saleItemRepository.GetSaleItemsBySaleId(saleId);

        if (!saleItemBySale.Any()) ErrorHelper.ThrowNotFoundException("No sale items found in this sale!");

        return saleItemBySale;
    }

    public async Task<SaleItems> CreateSaleItem(SaleItems saleItem)
    {
        return await _saleItemRepository.CreateSaleItem(saleItem);
    }

    public async Task<SaleItems> UpdateSaleItem(Guid saleItemId, SaleItems updateSaleItem)
    {
        var currentSaleItem = await GetSaleItemById(saleItemId);

        currentSaleItem.Quantity = updateSaleItem.Quantity;
        currentSaleItem.UnitPrice = updateSaleItem.UnitPrice;
        currentSaleItem.SubTotal = updateSaleItem.SubTotal;

        await _saleItemRepository.UpdateSaleItem(currentSaleItem);
        return currentSaleItem;
    }

    public async Task DeleteSaleItem(Guid saleItemId)
    {
        await _saleItemRepository.DeleteSaleItem(saleItemId);
    }
}
