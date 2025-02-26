using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Categories> Categories => _context.Categories;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Categories>> GetCategories()
    {
        throw new NotImplementedException();
    }

    public Task<Categories> GetCategoryById(Guid categoryId)
    {
        throw new NotImplementedException();
    }

    public Task<Categories> CreateCategory(Categories category)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCategory(Categories category)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategory(Guid categoryId)
    {
        throw new NotImplementedException();
    }
}
