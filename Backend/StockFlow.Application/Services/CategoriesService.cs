using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class CategoriesService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Categories>> GetCategories()
    {
        return await _categoryRepository.GetCategories();
    }

    public async Task<Categories> GetCategoryById(Guid categoryId)
    {
        var category = await _categoryRepository.GetCategoryById(categoryId);

        if (category == null) ErrorHelper.ThrowNotFoundException("Category not found!");

        return category;
    }

    public async Task<Categories> CreateCategory(Categories category)
    {
        return await _categoryRepository.CreateCategory(category);
    }

    public async Task<Categories> UpdateCategory(Guid categoryId, Categories updateCategory)
    {
        var currentCategory = await GetCategoryById(categoryId);

        currentCategory.Name = updateCategory.Name;

        await _categoryRepository.UpdateCategory(currentCategory);
        return currentCategory;
    }

    public async Task DeleteCategory(Guid categoryId)
    {
        await _categoryRepository.DeleteCategory(categoryId);
    }
}
