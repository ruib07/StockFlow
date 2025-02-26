using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class SalesService
{
    private readonly ISaleRepository _saleRepository;

    public SalesService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }
}
