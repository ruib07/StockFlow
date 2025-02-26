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

    public async Task<IEnumerable<Categories>> GetCategories()
    {
        return await Categories.ToListAsync();
    }

    public async Task<Categories> GetCategoryById(Guid categoryId)
    {
        return await Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public async Task<Categories> CreateCategory(Categories category)
    {
        await Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task UpdateCategory(Categories category)
    {
        Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategory(Guid categoryId)
    {
        var category = await GetCategoryById(categoryId);

        Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
