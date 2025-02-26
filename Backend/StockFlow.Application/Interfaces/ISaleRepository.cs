using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface ISaleRepository
{
    Task<IEnumerable<Sales>> GetSales();
    Task<Sales> GetSaleById(Guid saleId);
    Task<Sales> CreateSale(Sales sale);
    Task UpdateSale(Sales sale);
    Task DeleteSale(Guid saleId);
}
