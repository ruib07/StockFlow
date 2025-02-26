using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class CategoriesService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
}
