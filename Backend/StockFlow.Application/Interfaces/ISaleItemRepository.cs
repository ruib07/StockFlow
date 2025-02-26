using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface ISaleItemRepository
{
    Task<SaleItems> GetSaleItemById(Guid saleItemId);
    Task<IEnumerable<SaleItems>> GetSaleItemsBySaleId(Guid saleId);
    Task<SaleItems> CreateSaleItem(SaleItems saleItem);
    Task UpdateSaleItem(SaleItems saleItem);
    Task DeleteSaleItem(Guid saleItemId);
}
