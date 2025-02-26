using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class ProductsService
{
    private readonly IProductRepository _productRepository;

    public ProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
}
