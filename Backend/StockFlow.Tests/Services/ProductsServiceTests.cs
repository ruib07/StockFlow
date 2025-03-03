using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class ProductsServiceTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private ProductsService _productsService;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productsService = new ProductsService(_productRepositoryMock.Object);
    }

    [Test]
    public async Task GetProducts_ReturnsProducts()
    {
        var products = CreateProducts();

        _productRepositoryMock.Setup(repo => repo.GetProducts()).ReturnsAsync(products);

        var result = await _productsService.GetProducts();

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(products[0].Id));
            Assert.That(result.First().Name, Is.EqualTo(products[0].Name));
            Assert.That(result.First().Description, Is.EqualTo(products[0].Description));
            Assert.That(result.Last().Id, Is.EqualTo(products[1].Id));
            Assert.That(result.Last().Name, Is.EqualTo(products[1].Name));
            Assert.That(result.Last().Description, Is.EqualTo(products[1].Description));
        });
    }

    [Test]
    public async Task GetProductById_ReturnsProduct()
    {
        var product = CreateProducts().First();

        _productRepositoryMock.Setup(repo => repo.GetProductById(product.Id)).ReturnsAsync(product);

        var result = await _productsService.GetProductById(product.Id);

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
            Assert.That(result.CreatedAt, Is.EqualTo(product.CreatedAt));
        });
    }

    [Test]
    public void GetProductById_ReturnsNotFound_WhenProductNotFound()
    {
        _productRepositoryMock.Setup(repo => repo.GetProductById(It.IsAny<Guid>())).ReturnsAsync((Products)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _productsService.GetProductById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Product not found!"));
        });
    }

    [Test]
    public async Task GetProductsByCategoryId_ReturnsProduct()
    {
        var products = CreateProducts();
        var singleProductList = new List<Products>() { products[0] };

        _productRepositoryMock.Setup(repo => repo.GetProductsByCategoryId(products[0].CategoryId)).ReturnsAsync(singleProductList);

        var result = await _productsService.GetProductsByCategoryId(products[0].CategoryId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(products[0].Id));
            Assert.That(result.First().Name, Is.EqualTo(products[0].Name));
            Assert.That(result.First().Description, Is.EqualTo(products[0].Description));
            Assert.That(result.First().Price, Is.EqualTo(products[0].Price));
            Assert.That(result.First().Quantity, Is.EqualTo(products[0].Quantity));
            Assert.That(result.First().SupplierId, Is.EqualTo(products[0].SupplierId));
            Assert.That(result.First().CategoryId, Is.EqualTo(products[0].CategoryId));
            Assert.That(result.First().CreatedAt, Is.EqualTo(products[0].CreatedAt));
        });
    }

    [Test]
    public void GetProductsByCategoryId_ReturnsNotFound_WhenCategoryIdNotFound()
    {
        _productRepositoryMock.Setup(repo => repo.GetProductsByCategoryId(It.IsAny<Guid>())).ReturnsAsync((List<Products>)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _productsService.GetProductsByCategoryId(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("No products found in this category!"));
        });
    }

    [Test]
    public async Task CreateProduct_CreatesSuccessfully()
    {
        var product = CreateProducts().First();

        _productRepositoryMock.Setup(repo => repo.CreateProduct(product)).ReturnsAsync(product);

        var result = await _productsService.CreateProduct(product);

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
            Assert.That(result.CreatedAt, Is.EqualTo(product.CreatedAt));
        });
    }

    [Test]
    public async Task UpdateProduct_UpdatesSuccessfully()
    {
        var product = CreateProducts().First();
        var updateProduct = UpdateProduct(product.Id, product.SupplierId, product.CategoryId);

        _productRepositoryMock.Setup(repo => repo.CreateProduct(product)).ReturnsAsync(product);
        _productRepositoryMock.Setup(repo => repo.UpdateProduct(product)).Returns(Task.CompletedTask);
        _productRepositoryMock.Setup(repo => repo.GetProductById(product.Id)).ReturnsAsync(product);

        await _productsService.UpdateProduct(product.Id, updateProduct);
        var result = await _productsService.GetProductById(product.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateProduct.Id));
            Assert.That(result.Name, Is.EqualTo(updateProduct.Name));
            Assert.That(result.Description, Is.EqualTo(updateProduct.Description));
            Assert.That(result.Price, Is.EqualTo(updateProduct.Price));
            Assert.That(result.Quantity, Is.EqualTo(updateProduct.Quantity));
            Assert.That(result.SupplierId, Is.EqualTo(updateProduct.SupplierId));
            Assert.That(result.CategoryId, Is.EqualTo(updateProduct.CategoryId));
        });
    }

    [Test]
    public async Task DeleteProduct_DeletesSuccessfully()
    {
        var product = CreateProducts().First();

        _productRepositoryMock.Setup(repo => repo.CreateProduct(product)).ReturnsAsync(product);
        _productRepositoryMock.Setup(repo => repo.DeleteProduct(product.Id)).Returns(Task.CompletedTask);
        _productRepositoryMock.Setup(repo => repo.GetProductById(product.Id)).ReturnsAsync((Products)null);

        await _productsService.CreateProduct(product);
        await _productsService.DeleteProduct(product.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _productsService.GetProductById(product.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Product not found!"));
        });
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
                Price = 49.99m,
                Quantity = 2,
                SupplierId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            },
            new Products()
            {
                Id = Guid.NewGuid(),
                Name = "Product2 Test",
                Description = "Product2 Description",
                Price = 99.99m,
                Quantity = 4,
                SupplierId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    private static Products UpdateProduct(Guid id, Guid supplierId, Guid categoryId)
    {
        return new Products()
        {
            Id = id,
            Name = "Product Updated",
            Description = "Product Updated Description",
            Price = 149.99m,
            Quantity = 5,
            SupplierId = supplierId,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow.AddDays(2)
        };
    }

    #endregion
}
