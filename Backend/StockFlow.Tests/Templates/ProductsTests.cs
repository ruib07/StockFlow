using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class ProductsTests
{
    public static List<Products> CreateProducts()
    {
        return new List<Products>()
        {
            new Products()
            {
                Id = Guid.NewGuid(),
                Name = "Product1 Test",
                Description = "Product1 Description",
                Price = 49.99m,
                Quantity = 2,
                SupplierId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            },
            new Products()
            {
                Id = Guid.NewGuid(),
                Name = "Product2 Test",
                Description = "Product2 Description",
                Price = 99.99m,
                Quantity = 4,
                SupplierId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    public static Products UpdateProduct(Guid id, Guid supplierId, Guid categoryId)
    {
        return new Products()
        {
            Id = id,
            Name = "Product Updated",
            Description = "Product Updated Description",
            Price = 149.99m,
            Quantity = 5,
            SupplierId = supplierId,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow.AddDays(2)
        };
    }
}
