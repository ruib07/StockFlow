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
public class SalesControllerTests
{
    private Mock<ISaleRepository> _saleRepositoryMock;
    private SalesService _salesService;
    private SalesController _salesController;

    [SetUp]
    public void Setup()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _salesService = new SalesService(_saleRepositoryMock.Object);
        _salesController = new SalesController(_salesService);
    }

    [Test]
    public async Task GetSales_ReturnsOkResult_WithSales()
    {
        var sales = SalesTests.CreateSales();

        _saleRepositoryMock.Setup(repo => repo.GetSales()).ReturnsAsync(sales);

        var result = await _salesController.GetSales();
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<Sales>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(2));
            Assert.That(response.First().Id, Is.EqualTo(sales[0].Id));
            Assert.That(response.First().CustomerId, Is.EqualTo(sales[0].CustomerId));
            Assert.That(response.First().SaleDate, Is.EqualTo(sales[0].SaleDate));
            Assert.That(response.First().Total, Is.EqualTo(sales[0].Total));
            Assert.That(response.Last().Id, Is.EqualTo(sales[1].Id));
            Assert.That(response.Last().CustomerId, Is.EqualTo(sales[1].CustomerId));
            Assert.That(response.Last().SaleDate, Is.EqualTo(sales[1].SaleDate));
            Assert.That(response.Last().Total, Is.EqualTo(sales[1].Total));
        });
    }

    [Test]
    public async Task GetSaleById_ReturnsOkResult_WithSale()
    {
        var sale = SalesTests.CreateSales().First();

        _saleRepositoryMock.Setup(repo => repo.GetSaleById(sale.Id)).ReturnsAsync(sale);

        var result = await _salesController.GetSaleById(sale.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as Sales;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(sale.Id));
            Assert.That(response.CustomerId, Is.EqualTo(sale.CustomerId));
            Assert.That(response.SaleDate, Is.EqualTo(sale.SaleDate));
            Assert.That(response.Total, Is.EqualTo(sale.Total));
        });
    }

    [Test]
    public async Task CreateSale_ReturnsCreatedResult_WhenSaleIsCreated()
    {
        var newSale = SalesTests.CreateSales().First();

        _saleRepositoryMock.Setup(repo => repo.CreateSale(newSale)).ReturnsAsync(newSale);

        var result = await _salesController.CreateSale(newSale);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(response.Message, Is.EqualTo("Sale created successfully."));
            Assert.That(response.Id, Is.EqualTo(newSale.Id));
        });
    }

    [Test]
    public async Task UpdateSale_ReturnsOkResult_WhenSaleIsUpdated()
    {
        var sale = SalesTests.CreateSales().First();
        var updatedSale = SalesTests.UpdateSale(sale.Id, sale.CustomerId);

        _saleRepositoryMock.Setup(repo => repo.GetSaleById(sale.Id)).ReturnsAsync(sale);
        _saleRepositoryMock.Setup(repo => repo.UpdateSale(It.IsAny<Sales>())).Returns(Task.CompletedTask);

        var result = await _salesController.UpdateSale(sale.Id, updatedSale);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Sale updated successfully."));
        });
    }

    [Test]
    public async Task DeleteSale_ReturnsNoContentResult_WhenSaleIsDeleted()
    {
        var sale = SalesTests.CreateSales().First();

        _saleRepositoryMock.Setup(repo => repo.GetSaleById(sale.Id)).ReturnsAsync(sale);
        _saleRepositoryMock.Setup(repo => repo.DeleteSale(sale.Id)).Returns(Task.CompletedTask);

        var result = await _salesController.DeleteSale(sale.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
