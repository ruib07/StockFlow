using Moq;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;
using StockFlow.Server.Controllers;
using StockFlow.Tests.Templates;

namespace StockFlow.Tests.Controllers;

[TestFixture]
public class SaleItemsControllerTests
{
    private Mock<ISaleItemRepository> _saleItemRepositoryMock;
    private SaleItemsService _saleItemsService;
    private SaleItemsController _saleItemsController;

    [SetUp]
    public void Setup()
    {
        _saleItemRepositoryMock = new Mock<ISaleItemRepository>();
        _saleItemsService = new SaleItemsService(_saleItemRepositoryMock.Object);
        _saleItemsController = new SaleItemsController(_saleItemsService);
    }

    [Test]
    public async Task GetSaleItemById_ReturnsOkResult_WithSaleItem()
    {
        var saleItem = SaleItemsTests.CreateSaleItems().First();

        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemById(saleItem.Id)).ReturnsAsync(saleItem);

        var result = await _saleItemsController.GetSaleItemById(saleItem.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as SaleItems;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(saleItem.Id));
            Assert.That(response.SaleId, Is.EqualTo(saleItem.SaleId));
            Assert.That(response.ProductId, Is.EqualTo(saleItem.ProductId));
            Assert.That(response.Quantity, Is.EqualTo(saleItem.Quantity));
            Assert.That(response.UnitPrice, Is.EqualTo(saleItem.UnitPrice));
            Assert.That(response.SubTotal, Is.EqualTo(saleItem.SubTotal));
        });
    }

    [Test]
    public async Task GetSaleItemsBySaleId_ReturnsOkResult_WithSaleItems()
    {
        var saleItems = SaleItemsTests.CreateSaleItems();
        var singleSaleItemList = new List<SaleItems>() { saleItems[0] };

        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemsBySaleId(saleItems[0].SaleId)).ReturnsAsync(singleSaleItemList);

        var result = await _saleItemsController.GetSaleItemsBySaleId(saleItems[0].SaleId);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<SaleItems>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(1));
            Assert.That(response.First().Id, Is.EqualTo(saleItems[0].Id));
            Assert.That(response.First().SaleId, Is.EqualTo(saleItems[0].SaleId));
            Assert.That(response.First().ProductId, Is.EqualTo(saleItems[0].ProductId));
            Assert.That(response.First().Quantity, Is.EqualTo(saleItems[0].Quantity));
            Assert.That(response.First().UnitPrice, Is.EqualTo(saleItems[0].UnitPrice));
            Assert.That(response.First().SubTotal, Is.EqualTo(saleItems[0].SubTotal));
        });
    }

    [Test]
    public async Task CreateSaleItem_ReturnsCreatedResult_WhenSaleItemIsCreated()
    {
        var newSaleItem = SaleItemsTests.CreateSaleItems().First();

        _saleItemRepositoryMock.Setup(repo => repo.CreateSaleItem(newSaleItem)).ReturnsAsync(newSaleItem);

        var result = await _saleItemsController.CreateSaleItem(newSaleItem);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(response.Message, Is.EqualTo("Sale item created successfully."));
            Assert.That(response.Id, Is.EqualTo(newSaleItem.Id));
        });
    }

    [Test]
    public async Task UpdateSaleItem_ReturnsOkResult_WhenSaleItemIsUpdated()
    {
        var saleItem = SaleItemsTests.CreateSaleItems().First();
        var updatedSaleItems = SaleItemsTests.UpdateSaleItem(saleItem.Id, saleItem.SaleId, saleItem.ProductId);

        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemById(saleItem.Id)).ReturnsAsync(saleItem);
        _saleItemRepositoryMock.Setup(repo => repo.UpdateSaleItem(It.IsAny<SaleItems>())).Returns(Task.CompletedTask);

        var result = await _saleItemsController.UpdateSaleItem(saleItem.Id, updatedSaleItems);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Sale item updated successfully."));
        });
    }

    [Test]
    public async Task DeleteSaleItem_ReturnsNoContentResult_WhenSaleItemIsDeleted()
    {
        var saleItem = SaleItemsTests.CreateSaleItems().First();

        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemById(saleItem.Id)).ReturnsAsync(saleItem);
        _saleItemRepositoryMock.Setup(repo => repo.DeleteSaleItem(saleItem.Id)).Returns(Task.CompletedTask);

        var result = await _saleItemsController.DeleteSaleItem(saleItem.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
