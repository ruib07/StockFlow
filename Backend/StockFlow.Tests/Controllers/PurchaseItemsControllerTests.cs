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
public class PurchaseItemsControllerTests
{
    private Mock<IPurchaseItemRepository> _purchaseItemRepositoryMock;
    private PurchaseItemsService _purchaseItemsService;
    private PurchaseItemsController _purchaseItemsController;

    [SetUp]
    public void Setup()
    {
        _purchaseItemRepositoryMock = new Mock<IPurchaseItemRepository>();
        _purchaseItemsService = new PurchaseItemsService(_purchaseItemRepositoryMock.Object);
        _purchaseItemsController = new PurchaseItemsController(_purchaseItemsService);
    }

    [Test]
    public async Task GetPurchaseItemById_ReturnsOkResult_WithPurchaseItem()
    {
        var purchaseItem = PurchaseItemsTests.CreatePurchaseItems().First();

        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemById(purchaseItem.Id)).ReturnsAsync(purchaseItem);

        var result = await _purchaseItemsController.GetPurchaseItemById(purchaseItem.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as PurchaseItems;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(purchaseItem.Id));
            Assert.That(response.PurchaseId, Is.EqualTo(purchaseItem.PurchaseId));
            Assert.That(response.ProductId, Is.EqualTo(purchaseItem.ProductId));
            Assert.That(response.Quantity, Is.EqualTo(purchaseItem.Quantity));
            Assert.That(response.UnitPrice, Is.EqualTo(purchaseItem.UnitPrice));
            Assert.That(response.SubTotal, Is.EqualTo(purchaseItem.SubTotal));
        });
    }

    [Test]
    public async Task GetPurchaseItemsByPurchaseId_ReturnsOkResult_WithPurchaseItems()
    {
        var purchaseItems = PurchaseItemsTests.CreatePurchaseItems();
        var singlePurchaseItemList = new List<PurchaseItems>() { purchaseItems[0] };

        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemsByPurchaseId(purchaseItems[0].PurchaseId)).ReturnsAsync(singlePurchaseItemList);

        var result = await _purchaseItemsController.GetPurchaseItemsByPurchaseId(purchaseItems[0].PurchaseId);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<PurchaseItems>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(1));
            Assert.That(response.First().Id, Is.EqualTo(purchaseItems[0].Id));
            Assert.That(response.First().PurchaseId, Is.EqualTo(purchaseItems[0].PurchaseId));
        });
    }

    [Test]
    public async Task CreatePurchaseItem_ReturnsCreatedResult_WhenPurchaseItemIsCreated()
    {
        var newPurchaseItem = PurchaseItemsTests.CreatePurchaseItems().First();

        _purchaseItemRepositoryMock.Setup(repo => repo.CreatePurchaseItem(newPurchaseItem)).ReturnsAsync(newPurchaseItem);

        var result = await _purchaseItemsController.CreatePurchaseItem(newPurchaseItem);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(response.Message, Is.EqualTo("Purchase item created successfully."));
            Assert.That(response.Id, Is.EqualTo(newPurchaseItem.Id));
        });
    }

    [Test]
    public async Task UpdatePurchaseItem_ReturnsOkResult_WhenPurchaseItemIsUpdated()
    {
        var purchaseItem = PurchaseItemsTests.CreatePurchaseItems().First();
        var updatedPurchaseItem = PurchaseItemsTests.UpdatePurchaseItem(purchaseItem.Id, purchaseItem.PurchaseId, purchaseItem.ProductId);

        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemById(purchaseItem.Id)).ReturnsAsync(purchaseItem);
        _purchaseItemRepositoryMock.Setup(repo => repo.UpdatePurchaseItem(It.IsAny<PurchaseItems>())).Returns(Task.CompletedTask);

        var result = await _purchaseItemsController.UpdatePurchaseItem(purchaseItem.Id, updatedPurchaseItem);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Purchase item updated successfully."));
        });
    }

    [Test]
    public async Task DeletePurchaseItem_ReturnsNoContentResult_WhenPurchaseItemIsDeleted()
    {
        var purchaseItem = PurchaseItemsTests.CreatePurchaseItems().First();

        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemById(purchaseItem.Id)).ReturnsAsync(purchaseItem);
        _purchaseItemRepositoryMock.Setup(repo => repo.DeletePurchaseItem(purchaseItem.Id)).Returns(Task.CompletedTask);

        var result = await _purchaseItemsController.DeletePurchaseItem(purchaseItem.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
