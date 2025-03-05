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
public class PurchasesControllerTests
{
    private Mock<IPurchaseRepository> _purchaseRepositoryMock;
    private PurchasesService _purchasesService;
    private PurchasesController _purchasesController;

    [SetUp]
    public void Setup()
    {
        _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        _purchasesService = new PurchasesService(_purchaseRepositoryMock.Object);
        _purchasesController = new PurchasesController(_purchasesService);
    }

    [Test]
    public async Task GetPurchases_ReturnsOkResult_WithPurchases()
    {
        var purchases = PurchasesTests.CreatePurchases();

        _purchaseRepositoryMock.Setup(repo => repo.GetPurchases()).ReturnsAsync(purchases);

        var result = await _purchasesController.GetPurchases();
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<Purchases>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(2));
            Assert.That(response.First().Id, Is.EqualTo(purchases[0].Id));
            Assert.That(response.First().SupplierId, Is.EqualTo(purchases[0].SupplierId));
            Assert.That(response.First().PurchaseDate, Is.EqualTo(purchases[0].PurchaseDate));
            Assert.That(response.First().Total, Is.EqualTo(purchases[0].Total));
            Assert.That(response.Last().Id, Is.EqualTo(purchases[1].Id));
            Assert.That(response.Last().SupplierId, Is.EqualTo(purchases[1].SupplierId));
            Assert.That(response.Last().PurchaseDate, Is.EqualTo(purchases[1].PurchaseDate));
            Assert.That(response.Last().Total, Is.EqualTo(purchases[1].Total));
        });
    }

    [Test]
    public async Task GetPurchaseById_ReturnsOkResult_WithPurchase()
    {
        var purchase = PurchasesTests.CreatePurchases().First();

        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseById(purchase.Id)).ReturnsAsync(purchase);

        var result = await _purchasesController.GetPurchaseById(purchase.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as Purchases;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(purchase.Id));
            Assert.That(response.SupplierId, Is.EqualTo(purchase.SupplierId));
            Assert.That(response.PurchaseDate, Is.EqualTo(purchase.PurchaseDate));
            Assert.That(response.Total, Is.EqualTo(purchase.Total));
        });
    }

    [Test]
    public async Task CreatePurchase_ReturnsCreatedResult_WhenPurchaseIsCreated()
    {
        var newPurchase = PurchasesTests.CreatePurchases().First();

        _purchaseRepositoryMock.Setup(repo => repo.CreatePurchase(newPurchase)).ReturnsAsync(newPurchase);

        var result = await _purchasesController.CreatePurchase(newPurchase);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(response.Message, Is.EqualTo("Purchase created successfully."));
            Assert.That(response.Id, Is.EqualTo(newPurchase.Id));
        });
    }

    [Test]
    public async Task UpdatePurchase_ReturnsOkResult_WhenPurchaseIsUpdated()
    {
        var purchase = PurchasesTests.CreatePurchases().First();
        var updatedPurchase = PurchasesTests.UpdatePurchase(purchase.Id, purchase.SupplierId);

        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseById(purchase.Id)).ReturnsAsync(purchase);
        _purchaseRepositoryMock.Setup(repo => repo.UpdatePurchase(It.IsAny<Purchases>())).Returns(Task.CompletedTask);

        var result = await _purchasesController.UpdatePurchase(purchase.Id, updatedPurchase);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Purchase updated successfully."));
        });
    }

    [Test]
    public async Task DeletePurchase_ReturnsNoContentResult_WhenPurchaseIsDeleted()
    {
        var purchase = PurchasesTests.CreatePurchases().First();

        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseById(purchase.Id)).ReturnsAsync(purchase);
        _purchaseRepositoryMock.Setup(repo => repo.DeletePurchase(purchase.Id)).Returns(Task.CompletedTask);

        var result = await _purchasesController.DeletePurchase(purchase.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
