using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class PurchasesTests
{
    public static List<Purchases> CreatePurchases()
    {
        return new List<Purchases>()
        {
            new Purchases()
            {
                Id = Guid.NewGuid(),
                SupplierId = Guid.NewGuid(),
                PurchaseDate = DateTime.UtcNow,
                Total = 99.99m
            },
            new Purchases()
            {
                Id = Guid.NewGuid(),
                SupplierId = Guid.NewGuid(),
                PurchaseDate = DateTime.UtcNow.AddDays(2),
                Total = 129.99m
            }
        };
    }

    public static Purchases UpdatePurchase(Guid id, Guid supplierId)
    {
        return new Purchases()
        {
            Id = id,
            SupplierId = supplierId,
            PurchaseDate = DateTime.UtcNow.AddDays(3),
            Total = 89.99m
        };
    }
}
