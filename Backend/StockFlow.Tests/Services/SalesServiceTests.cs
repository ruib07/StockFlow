using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using StockFlow.Tests.Templates;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class SalesServiceTests
{
    private Mock<ISaleRepository> _saleRepositoryMock;
    private SalesService _salesService;

    [SetUp]
    public void Setup()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _salesService = new SalesService(_saleRepositoryMock.Object);
    }

    [Test]
    public async Task GetSales_ReturnsSales()
    {
        var sales = SalesTests.CreateSales();

        _saleRepositoryMock.Setup(repo => repo.GetSales()).ReturnsAsync(sales);

        var result = await _salesService.GetSales();

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(sales[0].Id));
            Assert.That(result.First().CustomerId, Is.EqualTo(sales[0].CustomerId));
            Assert.That(result.First().SaleDate, Is.EqualTo(sales[0].SaleDate));
            Assert.That(result.First().Total, Is.EqualTo(sales[0].Total));
            Assert.That(result.Last().Id, Is.EqualTo(sales[1].Id));
            Assert.That(result.Last().CustomerId, Is.EqualTo(sales[1].CustomerId));
            Assert.That(result.Last().SaleDate, Is.EqualTo(sales[1].SaleDate));
            Assert.That(result.Last().Total, Is.EqualTo(sales[1].Total));
        });
    }

    [Test]
    public async Task GetSaleById_ReturnsSale()
    {
        var sale = SalesTests.CreateSales().First();

        _saleRepositoryMock.Setup(repo => repo.GetSaleById(sale.Id)).ReturnsAsync(sale);

        var result = await _salesService.GetSaleById(sale.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(sale.Id));
            Assert.That(result.CustomerId, Is.EqualTo(sale.CustomerId));
            Assert.That(result.SaleDate, Is.EqualTo(sale.SaleDate));
            Assert.That(result.Total, Is.EqualTo(sale.Total));
        });
    }

    [Test]
    public void GetSaleById_ReturnsNotFound_WhenSaleNotFound()
    {
        _saleRepositoryMock.Setup(repo => repo.GetSaleById(It.IsAny<Guid>())).ReturnsAsync((Sales)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _salesService.GetSaleById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Sale not found!"));
        });
    }

    [Test]
    public async Task CreateSale_CreatesSuccessfully()
    {
        var sale = SalesTests.CreateSales().First();

        _saleRepositoryMock.Setup(repo => repo.CreateSale(sale)).ReturnsAsync(sale);

        var result = await _salesService.CreateSale(sale);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(sale.Id));
            Assert.That(result.CustomerId, Is.EqualTo(sale.CustomerId));
            Assert.That(result.SaleDate, Is.EqualTo(sale.SaleDate));
            Assert.That(result.Total, Is.EqualTo(sale.Total));
        });
    }

    [Test]
    public async Task UpdateSale_UpdatesSucessfully()
    {
        var sale = SalesTests.CreateSales().First();
        var updateSale = SalesTests.UpdateSale(sale.Id, sale.CustomerId);

        _saleRepositoryMock.Setup(repo => repo.CreateSale(sale)).ReturnsAsync(sale);
        _saleRepositoryMock.Setup(repo => repo.UpdateSale(sale)).Returns(Task.CompletedTask);
        _saleRepositoryMock.Setup(repo => repo.GetSaleById(sale.Id)).ReturnsAsync(sale);

        await _salesService.UpdateSale(sale.Id, updateSale);
        var result = await _salesService.GetSaleById(sale.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateSale.Id));
            Assert.That(result.CustomerId, Is.EqualTo(updateSale.CustomerId));
            Assert.That(result.SaleDate, Is.EqualTo(updateSale.SaleDate));
            Assert.That(result.Total, Is.EqualTo(updateSale.Total));
        });
    }

    [Test]
    public async Task DeleteSale_DeletesSuccessfully()
    {
        var sale = SalesTests.CreateSales().First();

        _saleRepositoryMock.Setup(repo => repo.CreateSale(sale)).ReturnsAsync(sale);
        _saleRepositoryMock.Setup(repo => repo.DeleteSale(sale.Id)).Returns(Task.CompletedTask);
        _saleRepositoryMock.Setup(repo => repo.GetSaleById(sale.Id)).ReturnsAsync((Sales)null);

        await _salesService.CreateSale(sale);
        await _salesService.DeleteSale(sale.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _salesService.GetSaleById(sale.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Sale not found!"));
        });
    }
}
