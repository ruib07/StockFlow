using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class CategoriesTests
{
    public static List<Categories> CreateCategories()
    {
        return new List<Categories>()
        {
            new Categories()
            {
                Id = Guid.NewGuid(),
                Name = "Category1 Test"
            },
            new Categories()
            {
                Id = Guid.NewGuid(),
                Name = "Category2 Test"
            }
        };
    }

    public static Categories UpdateCategory(Guid id)
    {
        return new Categories()
        {
            Id = id,
            Name = "Category Updated"
        };
    }
}
