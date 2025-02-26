using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface ISaleRepository
{
    Task<IEnumerable<Sales>> GetSales();
    Task<Sales> GetSaleById(Guid saleId);
    Task<Sales> CreateSale(Sales sales);
    Task UpdateSale(Sales sales);
    Task DeleteSale(Guid saleId);
}
