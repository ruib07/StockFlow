using Moq;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using StockFlow.Server.Controllers;
using StockFlow.Tests.Templates;
using StockFlow.Domain.DTOs;

namespace StockFlow.Tests.Controllers;

[TestFixture]
public class ProductsControllerTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private ProductsService _productsService;
    private ProductsController _productsController;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productsService = new ProductsService(_productRepositoryMock.Object);
        _productsController = new ProductsController(_productsService);
    }

    [Test]
    public async Task GetProducts_ReturnsOkResult_WithProducts()
    {
        var products = ProductsTests.CreateProducts();

        _productRepositoryMock.Setup(repo => repo.GetProducts()).ReturnsAsync(products);

        var result = await _productsController.GetProducts();
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<Products>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(2));
            Assert.That(response.First().Id, Is.EqualTo(products[0].Id));
            Assert.That(response.First().Name, Is.EqualTo(products[0].Name));
            Assert.That(response.Last().Id, Is.EqualTo(products[1].Id));
            Assert.That(response.Last().Name, Is.EqualTo(products[1].Name));
        });
    }

    [Test]
    public async Task GetProductById_ReturnsOkResult_WithProduct()
    {
        var product = ProductsTests.CreateProducts().First();

        _productRepositoryMock.Setup(repo => repo.GetProductById(product.Id)).ReturnsAsync(product);

        var result = await _productsController.GetProductById(product.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as Products;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(product.Id));
            Assert.That(response.Name, Is.EqualTo(product.Name));
            Assert.That(response.Description, Is.EqualTo(product.Description));
            Assert.That(response.Price, Is.EqualTo(product.Price));
            Assert.That(response.Quantity, Is.EqualTo(product.Quantity));
            Assert.That(response.SupplierId, Is.EqualTo(product.SupplierId));
            Assert.That(response.CategoryId, Is.EqualTo(product.CategoryId));
        });
    }

    [Test]
    public async Task GetProductsByCategoryId_ReturnsOkResult_WithProducts()
    {
        var products = ProductsTests.CreateProducts();
        var singleProductList = new List<Products>() { products[0] };

        _productRepositoryMock.Setup(repo => repo.GetProductsByCategoryId(products[0].CategoryId)).ReturnsAsync(singleProductList);

        var result = await _productsController.GetProductsByCategoryId(products[0].CategoryId);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<Products>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(1));
            Assert.That(response.First().Id, Is.EqualTo(products[0].Id));
            Assert.That(response.First().Name, Is.EqualTo(products[0].Name));
        });
    }

    [Test]
    public async Task CreateProduct_ReturnsCreatedResult_WhenProductIsCreated()
    {
        var newProduct = ProductsTests.CreateProducts().First();

        _productRepositoryMock.Setup(repo => repo.CreateProduct(newProduct)).ReturnsAsync(newProduct);

        var result = await _productsController.CreateProduct(newProduct);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(response.Message, Is.EqualTo("Product created successfully."));
            Assert.That(response.Id, Is.EqualTo(newProduct.Id));
        });
    }

    [Test]
    public async Task UpdateProduct_ReturnsOkResult_WhenProductIsUpdated()
    {
        var product = ProductsTests.CreateProducts().First();
        var updatedProduct = ProductsTests.UpdateProduct(product.Id, product.SupplierId, product.CategoryId);

        _productRepositoryMock.Setup(repo => repo.GetProductById(product.Id)).ReturnsAsync(product);
        _productRepositoryMock.Setup(repo => repo.UpdateProduct(It.IsAny<Products>())).Returns(Task.CompletedTask);

        var result = await _productsController.UpdateProduct(product.Id, updatedProduct);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Product updated successfully."));
        });
    }

    [Test]
    public async Task DeleteProduct_ReturnsNoContentResult_WhenProductIsDeleted()
    {
        var product = ProductsTests.CreateProducts().First();

        _productRepositoryMock.Setup(repo => repo.GetProductById(product.Id)).ReturnsAsync(product);
        _productRepositoryMock.Setup(repo => repo.DeleteProduct(product.Id)).Returns(Task.CompletedTask);

        var result = await _productsController.DeleteProduct(product.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
