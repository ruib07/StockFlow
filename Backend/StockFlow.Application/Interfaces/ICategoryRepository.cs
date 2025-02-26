using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Categories>> GetCategories();
    Task<Categories> GetCategoryById(Guid categoryId);
    Task<Categories> CreateCategory(Categories category);
    Task UpdateCategory(Categories category);
    Task DeleteCategory(Guid categoryId);
}
