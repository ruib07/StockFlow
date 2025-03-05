using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class PurchaseItemsTests
{
    public static List<PurchaseItems> CreatePurchaseItems()
    {
        return new List<PurchaseItems>()
        {
            new PurchaseItems()
            {
                Id = Guid.NewGuid(),
                PurchaseId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 49.99m,
                SubTotal = 49.99m
            },
            new PurchaseItems()
            {
                Id = Guid.NewGuid(),
                PurchaseId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 99.99m,
                SubTotal = 198.98m
            }
        };
    }

    public static PurchaseItems UpdatePurchaseItem(Guid id, Guid purchaseId, Guid productId)
    {
        return new PurchaseItems()
        {
            Id = id,
            PurchaseId = purchaseId,
            ProductId = productId,
            Quantity = 2,
            UnitPrice = 49.99m,
            SubTotal = 98.98m
        };
    }
}
