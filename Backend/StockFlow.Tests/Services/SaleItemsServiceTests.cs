using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using StockFlow.Tests.Templates;
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
        var saleItem = SaleItemsTests.CreateSaleItems().First();

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
        var saleItems = SaleItemsTests.CreateSaleItems();
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
        var saleItem = SaleItemsTests.CreateSaleItems().First();

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
        var saleItem = SaleItemsTests.CreateSaleItems().First();
        var updateSaleItem = SaleItemsTests.UpdateSaleItem(saleItem.Id, saleItem.SaleId, saleItem.ProductId);

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
        var saleItem = SaleItemsTests.CreateSaleItems().First();

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
}
