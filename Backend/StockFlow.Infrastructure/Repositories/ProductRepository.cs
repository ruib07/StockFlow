using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Products> Products => _context.Products;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Products>> GetProducts()
    {
        return await Products.ToListAsync();
    }

    public async Task<Products> GetProductById(Guid productId)
    {
        return await Products.FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<IEnumerable<Products>> GetProductsByCategoryId(Guid categoryId)
    {
        return await Products.AsNoTracking().Where(p => p.CategoryId == categoryId).ToListAsync();
    }

    public async Task<Products> CreateProduct(Products product)
    {
        await Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task UpdateProduct(Products product)
    {
        Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProduct(Guid productId)
    {
        var product = await GetProductById(productId);

        Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
