using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class SalesService
{
    private readonly ISaleRepository _saleRepository;

    public SalesService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IEnumerable<Sales>> GetSales()
    {
        return await _saleRepository.GetSales();
    }

    public async Task<Sales> GetSaleById(Guid saleId)
    {
        var sale = await _saleRepository.GetSaleById(saleId);

        if (sale == null) ErrorHelper.ThrowNotFoundException("Sale not found!");

        return sale;
    }

    public async Task<Sales> CreateSale(Sales sale)
    {
        return await _saleRepository.CreateSale(sale);
    }

    public async Task<Sales> UpdateSale(Guid saleId, Sales updateSale)
    {
        var currentSale = await GetSaleById(saleId);

        currentSale.SaleDate = updateSale.SaleDate;
        currentSale.Total = updateSale.Total;

        await _saleRepository.UpdateSale(currentSale);
        return currentSale;
    }

    public async Task DeleteSale(Guid saleId)
    {
        await _saleRepository.DeleteSale(saleId);
    }
}
