using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class ProductRepositoryTests
{
    private ProductRepository _productRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _productRepository = new ProductRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetProducts_ReturnsProducts()
    {
        var products = CreateProducts();
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var result = await _productRepository.GetProducts();

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(products[0].Id));
            Assert.That(result.First().Name, Is.EqualTo(products[0].Name));
            Assert.That(result.Last().Id, Is.EqualTo(products[1].Id));
            Assert.That(result.Last().Name, Is.EqualTo(products[1].Name));
        });
    }

    [Test]
    public async Task GetProductById_ReturnsProduct()
    {
        var product = CreateProducts().First();

        await _productRepository.CreateProduct(product);

        var result = await _productRepository.GetProductById(product.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(product.Id));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Description, Is.EqualTo(product.Description));
            Assert.That(result.Price, Is.EqualTo(product.Price));
            Assert.That(result.Quantity, Is.EqualTo(product.Quantity));
            Assert.That(result.SupplierId, Is.EqualTo(product.SupplierId));
            Assert.That(result.CategoryId, Is.EqualTo(product.CategoryId));
        });
    }

    [Test]
    public async Task GetProductsByCategoryId_ReturnsProduct()
    {
        var products = CreateProducts();
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var result = await _productRepository.GetProductsByCategoryId(products[0].CategoryId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Id, Is.EqualTo(products[0].Id));
            Assert.That(result.First().Name, Is.EqualTo(products[0].Name));
            Assert.That(result.First().Description, Is.EqualTo(products[0].Description));
            Assert.That(result.First().Price, Is.EqualTo(products[0].Price));
            Assert.That(result.First().Quantity, Is.EqualTo(products[0].Quantity));
            Assert.That(result.First().SupplierId, Is.EqualTo(products[0].SupplierId));
            Assert.That(result.First().CategoryId, Is.EqualTo(products[0].CategoryId));
        });
    }

    [Test]
    public async Task CreateProduct_CreatesSuccessfully()
    {
        var newProduct = CreateProducts().First();

        var result = await _productRepository.CreateProduct(newProduct);
        var addedProduct = await _productRepository.GetProductById(newProduct.Id);

        Assert.That(addedProduct, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedProduct.Id, Is.EqualTo(newProduct.Id));
            Assert.That(addedProduct.Name, Is.EqualTo(newProduct.Name));
            Assert.That(addedProduct.Description, Is.EqualTo(newProduct.Description));
            Assert.That(addedProduct.Price, Is.EqualTo(newProduct.Price));
            Assert.That(addedProduct.Quantity, Is.EqualTo(newProduct.Quantity));
            Assert.That(addedProduct.SupplierId, Is.EqualTo(newProduct.SupplierId));
            Assert.That(addedProduct.CategoryId, Is.EqualTo(newProduct.CategoryId));
        });
    }

    [Test]
    public async Task UpdateProduct_UpdatesSuccessfully()
    {
        var existingProduct = CreateProducts().First();
        await _productRepository.CreateProduct(existingProduct);

        _context.Entry(existingProduct).State = EntityState.Detached;

        var updatedProduct = UpdateProduct(existingProduct.Id, existingProduct.SupplierId, existingProduct.CategoryId);

        await _productRepository.UpdateProduct(updatedProduct);
        var retrievedUpdatedProduct = await _productRepository.GetProductById(existingProduct.Id);

        Assert.That(retrievedUpdatedProduct, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedProduct.Id, Is.EqualTo(updatedProduct.Id));
            Assert.That(retrievedUpdatedProduct.Name, Is.EqualTo(updatedProduct.Name));
            Assert.That(retrievedUpdatedProduct.Description, Is.EqualTo(updatedProduct.Description));
            Assert.That(retrievedUpdatedProduct.Price, Is.EqualTo(updatedProduct.Price));
            Assert.That(retrievedUpdatedProduct.Quantity, Is.EqualTo(updatedProduct.Quantity));
            Assert.That(retrievedUpdatedProduct.SupplierId, Is.EqualTo(updatedProduct.SupplierId));
            Assert.That(retrievedUpdatedProduct.CategoryId, Is.EqualTo(updatedProduct.CategoryId));
        });
    }

    [Test]
    public async Task DeleteProduct_DeletesSuccessfully()
    {
        var existingProduct = CreateProducts().First();

        await _productRepository.CreateProduct(existingProduct);
        await _productRepository.DeleteProduct(existingProduct.Id);
        var retrivedEmptyProduct = await _productRepository.GetProductById(existingProduct.Id);

        Assert.That(retrivedEmptyProduct, Is.Null);
    }

    #region Private Methods

    private static List<Products> CreateProducts()
    {
        return new List<Products>()
        {
            new Products()
            {
                Id = Guid.NewGuid(),
                Name = "Product1 Test",
                Description = "Product1 Description",
                Price = 99.99m,
                Quantity = 2,
                SupplierId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
            },
            new Products()
            {
                Id = Guid.NewGuid(),
                Name = "Product2 Test",
                Description = "Product2 Description",
                Price = 99.99m,
                Quantity = 2,
                SupplierId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
            }
        };
    }

    private static Products UpdateProduct(Guid id, Guid supplierId, Guid categoryId)
    {
        return new Products()
        {
            Id = id,
            Name = "Product1 Test",
            Description = "Product1 Description",
            Price = 99.99m,
            Quantity = 2,
            SupplierId = supplierId,
            CategoryId = categoryId,
        };
    }

    #endregion
}
