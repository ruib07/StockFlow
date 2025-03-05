using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class SalesTests
{
    public static List<Sales> CreateSales()
    {
        return new List<Sales>()
        {
            new Sales()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Total = 99.99m
            },
            new Sales()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow.AddDays(2),
                Total = 129.99m
            }
        };
    }

    public static Sales UpdateSale(Guid id, Guid customerId)
    {
        return new Sales()
        {
            Id = id,
            CustomerId = customerId,
            SaleDate = DateTime.UtcNow.AddDays(3),
            Total = 89.99m
        };
    }
}
