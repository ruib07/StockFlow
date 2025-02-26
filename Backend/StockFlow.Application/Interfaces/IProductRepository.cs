using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Products>> GetProducts();
    Task<Products> GetProductById(Guid productId);
    Task<IEnumerable<Products>> GetProductsByCategoryId(Guid categoryId);
    Task<Products> CreateProduct(Products product);
    Task UpdateProduct(Products product);
    Task DeleteProduct(Guid productId);
}
