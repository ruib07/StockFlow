using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class SaleItemsTests
{
    public static List<SaleItems> CreateSaleItems()
    {
        return new List<SaleItems>()
        {
            new SaleItems()
            {
                Id = Guid.NewGuid(),
                SaleId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 49.99m,
                SubTotal = 49.99m
            },
            new SaleItems()
            {
                Id = Guid.NewGuid(),
                SaleId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 99.99m,
                SubTotal = 198.98m
            }
        };
    }

    public static SaleItems UpdateSaleItem(Guid id, Guid saleId, Guid productId)
    {
        return new SaleItems()
        {
            Id = id,
            SaleId = saleId,
            ProductId = productId,
            Quantity = 2,
            UnitPrice = 49.99m,
            SubTotal = 98.98m
        };
    }
}
