using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class ProductsService
{
    private readonly IProductRepository _productRepository;

    public ProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Products>> GetProducts()
    {
        return await _productRepository.GetProducts();
    }

    public async Task<Products> GetProductById(Guid productId)
    {
        var product = await _productRepository.GetProductById(productId);

        if (product == null) ErrorHelper.ThrowNotFoundException("Product not found!");

        return product;
    }

    public async Task<IEnumerable<Products>> GetProductsByCategoryId(Guid categoryId)
    {
        var productsByCategory = await _productRepository.GetProductsByCategoryId(categoryId);

        if (productsByCategory == null || !productsByCategory.Any()) ErrorHelper.ThrowNotFoundException("No products found in this category!");

        return productsByCategory;
    }

    public async Task<Products> CreateProduct(Products product)
    {
        return await _productRepository.CreateProduct(product);
    }

    public async Task<Products> UpdateProduct(Guid productId, Products updateProduct)
    {
        var currentProduct = await GetProductById(productId);

        currentProduct.Name = updateProduct.Name;
        currentProduct.Description = updateProduct.Description;
        currentProduct.Price = updateProduct.Price;
        currentProduct.Quantity = updateProduct.Quantity;

        await _productRepository.UpdateProduct(currentProduct);
        return currentProduct;
    }

    public async Task DeleteProduct(Guid productId)
    {
        await _productRepository.DeleteProduct(productId);
    }
}
