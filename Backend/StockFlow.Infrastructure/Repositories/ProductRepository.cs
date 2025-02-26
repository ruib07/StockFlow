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

    public Task<IEnumerable<Products>> GetProducts()
    {
        throw new NotImplementedException();
    }

    public Task<Products> GetProductById(Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Products>> GetProductsByCategoryId(Guid categoryId)
    {
        throw new NotImplementedException();
    }

    public Task<Products> CreateProduct(Products product)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProduct(Products product)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProduct(Guid productId)
    {
        throw new NotImplementedException();
    }
}
