using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface ISaleItemRepository
{
    Task<IEnumerable<SaleItems>> GetSaleItemsBySaleId(Guid saleId);
    Task<SaleItems> CreateSaleItem(SaleItems item);
    Task UpdateSaleItem(SaleItems item);
    Task DeleteSaleItem(Guid saleItemId);
}
