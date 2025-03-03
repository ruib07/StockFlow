using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class PurchaseItemsServiceTests
{
    private Mock<IPurchaseItemRepository> _purchaseItemRepositoryMock;
    private PurchaseItemsService _purchaseItemsService;

    [SetUp]
    public void Setup()
    {
        _purchaseItemRepositoryMock = new Mock<IPurchaseItemRepository>();
        _purchaseItemsService = new PurchaseItemsService(_purchaseItemRepositoryMock.Object);
    }

    [Test]
    public async Task GetPurchaseItemById_ReturnsPurchaseItem()
    {
        var purchaseItem = CreatePurchaseItems().First();

        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemById(purchaseItem.Id)).ReturnsAsync(purchaseItem);

        var result = await _purchaseItemsService.GetPurchaseItemById(purchaseItem.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(purchaseItem.Id));
            Assert.That(result.PurchaseId, Is.EqualTo(purchaseItem.PurchaseId));
            Assert.That(result.ProductId, Is.EqualTo(purchaseItem.ProductId));
            Assert.That(result.Quantity, Is.EqualTo(purchaseItem.Quantity));
            Assert.That(result.UnitPrice, Is.EqualTo(purchaseItem.UnitPrice));
            Assert.That(result.SubTotal, Is.EqualTo(purchaseItem.SubTotal));
        });
    }

    [Test]
    public void GetPurchaseItemById_ReturnsNotFound_WhenPurchaseItemNotFound()
    {
        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemById(It.IsAny<Guid>())).ReturnsAsync((PurchaseItems)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _purchaseItemsService.GetPurchaseItemById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Purchase item not found!"));
        });
    }

    [Test]
    public async Task GetPurchaseItemsByPurchaseId_ReturnsPurchaseItem()
    {
        var purchaseItems = CreatePurchaseItems();
        var singlePurchaseItemList = new List<PurchaseItems>() { purchaseItems[0] };

        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemsByPurchaseId(purchaseItems[0].PurchaseId)).ReturnsAsync(singlePurchaseItemList);

        var result = await _purchaseItemsService.GetPurchaseItemsByPurchaseId(purchaseItems[0].PurchaseId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(purchaseItems[0].Id));
            Assert.That(result.First().PurchaseId, Is.EqualTo(purchaseItems[0].PurchaseId));
            Assert.That(result.First().ProductId, Is.EqualTo(purchaseItems[0].ProductId));
            Assert.That(result.First().Quantity, Is.EqualTo(purchaseItems[0].Quantity));
            Assert.That(result.First().UnitPrice, Is.EqualTo(purchaseItems[0].UnitPrice));
            Assert.That(result.First().SubTotal, Is.EqualTo(purchaseItems[0].SubTotal));
        });
    }

    [Test]
    public void GetPurchaseItemsByPurchaseId_ReturnsNotFound_WhenPurchaseIdNotFound()
    {
        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemsByPurchaseId(It.IsAny<Guid>())).ReturnsAsync((List<PurchaseItems>)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _purchaseItemsService.GetPurchaseItemsByPurchaseId(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("No purchase items found in this purchase!"));
        });
    }

    [Test]
    public async Task CreatePurchaseItem_CreatesSuccessfully()
    {
        var purchaseItem = CreatePurchaseItems().First();

        _purchaseItemRepositoryMock.Setup(repo => repo.CreatePurchaseItem(purchaseItem)).ReturnsAsync(purchaseItem);

        var result = await _purchaseItemsService.CreatePurchaseItem(purchaseItem);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(purchaseItem.Id));
            Assert.That(result.PurchaseId, Is.EqualTo(purchaseItem.PurchaseId));
            Assert.That(result.ProductId, Is.EqualTo(purchaseItem.ProductId));
            Assert.That(result.Quantity, Is.EqualTo(purchaseItem.Quantity));
            Assert.That(result.UnitPrice, Is.EqualTo(purchaseItem.UnitPrice));
            Assert.That(result.SubTotal, Is.EqualTo(purchaseItem.SubTotal));
        });
    }

    [Test]
    public async Task UpdatePurchaseItem_UpdatesSuccessfully()
    {
        var purchaseItem = CreatePurchaseItems().First();
        var updatePurchaseItem = UpdatePurchaseItem(purchaseItem.Id, purchaseItem.PurchaseId, purchaseItem.ProductId);

        _purchaseItemRepositoryMock.Setup(repo => repo.CreatePurchaseItem(purchaseItem)).ReturnsAsync(purchaseItem);
        _purchaseItemRepositoryMock.Setup(repo => repo.UpdatePurchaseItem(purchaseItem)).Returns(Task.CompletedTask);
        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemById(purchaseItem.Id)).ReturnsAsync(purchaseItem);

        await _purchaseItemsService.UpdatePurchaseItem(purchaseItem.Id, updatePurchaseItem);
        var result = await _purchaseItemsService.GetPurchaseItemById(purchaseItem.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updatePurchaseItem.Id));
            Assert.That(result.PurchaseId, Is.EqualTo(updatePurchaseItem.PurchaseId));
            Assert.That(result.ProductId, Is.EqualTo(updatePurchaseItem.ProductId));
            Assert.That(result.Quantity, Is.EqualTo(updatePurchaseItem.Quantity));
            Assert.That(result.UnitPrice, Is.EqualTo(updatePurchaseItem.UnitPrice));
            Assert.That(result.SubTotal, Is.EqualTo(updatePurchaseItem.SubTotal));
        });
    }

    [Test]
    public async Task DeletePurchaseItem_DeletesSuccessfully()
    {
        var purchaseItem = CreatePurchaseItems().First();

        _purchaseItemRepositoryMock.Setup(repo => repo.CreatePurchaseItem(purchaseItem)).ReturnsAsync(purchaseItem);
        _purchaseItemRepositoryMock.Setup(repo => repo.DeletePurchaseItem(purchaseItem.Id)).Returns(Task.CompletedTask);
        _purchaseItemRepositoryMock.Setup(repo => repo.GetPurchaseItemById(purchaseItem.Id)).ReturnsAsync((PurchaseItems)null);

        await _purchaseItemsService.CreatePurchaseItem(purchaseItem);
        await _purchaseItemsService.DeletePurchaseItem(purchaseItem.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _purchaseItemsService.GetPurchaseItemById(purchaseItem.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Purchase item not found!"));
        });
    }

    #region Private Methods

    private static List<PurchaseItems> CreatePurchaseItems()
    {
        return new List<PurchaseItems>()
        {
            new PurchaseItems()
            {
                Id = Guid.NewGuid(),
                PurchaseId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 49.99m,
                SubTotal = 49.99m
            },
            new PurchaseItems()
            {
                Id = Guid.NewGuid(),
                PurchaseId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 99.99m,
                SubTotal = 198.98m
            }
        };
    }

    private static PurchaseItems UpdatePurchaseItem(Guid id, Guid purchaseId, Guid productId)
    {
        return new PurchaseItems()
        {
            Id = id,
            PurchaseId = purchaseId,
            ProductId = productId,
            Quantity = 2,
            UnitPrice = 49.99m,
            SubTotal = 98.98m
        };
    }

    #endregion
}
