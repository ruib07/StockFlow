using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class SaleItemsServiceTests
{
    private Mock<ISaleItemRepository> _saleItemRepositoryMock;
    private SaleItemsService _saleItemsService;

    [SetUp]
    public void Setup()
    {
        _saleItemRepositoryMock = new Mock<ISaleItemRepository>();
        _saleItemsService = new SaleItemsService(_saleItemRepositoryMock.Object);
    }


    [Test]
    public async Task GetSaleItemById_ReturnsSaleItem()
    {
        var saleItem = CreateSaleItems().First();

        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemById(saleItem.Id)).ReturnsAsync(saleItem);

        var result = await _saleItemsService.GetSaleItemById(saleItem.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(saleItem.Id));
            Assert.That(result.SaleId, Is.EqualTo(saleItem.SaleId));
            Assert.That(result.ProductId, Is.EqualTo(saleItem.ProductId));
            Assert.That(result.Quantity, Is.EqualTo(saleItem.Quantity));
            Assert.That(result.UnitPrice, Is.EqualTo(saleItem.UnitPrice));
            Assert.That(result.SubTotal, Is.EqualTo(saleItem.SubTotal));
        });
    }

    [Test]
    public void GetSaleItemById_ReturnsNotFound_WhenSaleItemNotFound()
    {
        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemById(It.IsAny<Guid>())).ReturnsAsync((SaleItems)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _saleItemsService.GetSaleItemById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Sale item not found!"));
        });
    }

    [Test]
    public async Task GetSaleItemsBySaleId_ReturnsSaleItem()
    {
        var saleItems = CreateSaleItems();
        var singleSaleItemList = new List<SaleItems>() { saleItems[0] };

        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemsBySaleId(saleItems[0].SaleId)).ReturnsAsync(singleSaleItemList);

        var result = await _saleItemsService.GetSaleItemsBySaleId(saleItems[0].SaleId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(saleItems[0].Id));
            Assert.That(result.First().SaleId, Is.EqualTo(saleItems[0].SaleId));
            Assert.That(result.First().ProductId, Is.EqualTo(saleItems[0].ProductId));
            Assert.That(result.First().Quantity, Is.EqualTo(saleItems[0].Quantity));
            Assert.That(result.First().UnitPrice, Is.EqualTo(saleItems[0].UnitPrice));
            Assert.That(result.First().SubTotal, Is.EqualTo(saleItems[0].SubTotal));
        });
    }

    [Test]
    public void GetSaleItemsBySaleId_ReturnsNotFound_WhenSaleIdNotFound()
    {
        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemsBySaleId(It.IsAny<Guid>())).ReturnsAsync((List<SaleItems>)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _saleItemsService.GetSaleItemsBySaleId(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("No sale items found in this sale!"));
        });
    }

    [Test]
    public async Task CreateSaleItem_CreatesSuccessfully()
    {
        var saleItem = CreateSaleItems().First();

        _saleItemRepositoryMock.Setup(repo => repo.CreateSaleItem(saleItem)).ReturnsAsync(saleItem);

        var result = await _saleItemsService.CreateSaleItem(saleItem);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(saleItem.Id));
            Assert.That(result.SaleId, Is.EqualTo(saleItem.SaleId));
            Assert.That(result.ProductId, Is.EqualTo(saleItem.ProductId));
            Assert.That(result.Quantity, Is.EqualTo(saleItem.Quantity));
            Assert.That(result.UnitPrice, Is.EqualTo(saleItem.UnitPrice));
            Assert.That(result.SubTotal, Is.EqualTo(saleItem.SubTotal));
        });
    }

    [Test]
    public async Task UpdateSaleItem_UpdatesSuccessfully()
    {
        var saleItem = CreateSaleItems().First();
        var updateSaleItem = UpdateSaleItem(saleItem.Id, saleItem.SaleId, saleItem.ProductId);

        _saleItemRepositoryMock.Setup(repo => repo.CreateSaleItem(saleItem)).ReturnsAsync(saleItem);
        _saleItemRepositoryMock.Setup(repo => repo.UpdateSaleItem(saleItem)).Returns(Task.CompletedTask);
        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemById(saleItem.Id)).ReturnsAsync(saleItem);

        await _saleItemsService.UpdateSaleItem(saleItem.Id, updateSaleItem);
        var result = await _saleItemsService.GetSaleItemById(saleItem.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateSaleItem.Id));
            Assert.That(result.SaleId, Is.EqualTo(updateSaleItem.SaleId));
            Assert.That(result.ProductId, Is.EqualTo(updateSaleItem.ProductId));
            Assert.That(result.Quantity, Is.EqualTo(updateSaleItem.Quantity));
            Assert.That(result.UnitPrice, Is.EqualTo(updateSaleItem.UnitPrice));
            Assert.That(result.SubTotal, Is.EqualTo(updateSaleItem.SubTotal));
        });
    }

    [Test]
    public async Task DeleteSaleItem_DeletesSuccessfully()
    {
        var saleItem = CreateSaleItems().First();

        _saleItemRepositoryMock.Setup(repo => repo.CreateSaleItem(saleItem)).ReturnsAsync(saleItem);
        _saleItemRepositoryMock.Setup(repo => repo.DeleteSaleItem(saleItem.Id)).Returns(Task.CompletedTask);
        _saleItemRepositoryMock.Setup(repo => repo.GetSaleItemById(saleItem.Id)).ReturnsAsync((SaleItems)null);

        await _saleItemsService.CreateSaleItem(saleItem);
        await _saleItemsService.DeleteSaleItem(saleItem.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _saleItemsService.GetSaleItemById(saleItem.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Sale item not found!"));
        });
    }

    #region Private Methods

    private static List<SaleItems> CreateSaleItems()
    {
        return new List<SaleItems>()
        {
            new SaleItems()
            {
                Id = Guid.NewGuid(),
                SaleId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 49.99m,
                SubTotal = 49.99m
            },
            new SaleItems()
            {
                Id = Guid.NewGuid(),
                SaleId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 99.99m,
                SubTotal = 198.98m
            }
        };
    }

    private static SaleItems UpdateSaleItem(Guid id, Guid saleId, Guid productId)
    {
        return new SaleItems()
        {
            Id = id,
            SaleId = saleId,
            ProductId = productId,
            Quantity = 2,
            UnitPrice = 49.99m,
            SubTotal = 98.98m
        };
    }

    #endregion
}
