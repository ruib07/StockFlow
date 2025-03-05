using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using StockFlow.Tests.Templates;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class PurchasesServiceTests
{
    private Mock<IPurchaseRepository> _purchaseRepositoryMock;
    private PurchasesService _purchaseService;

    [SetUp]
    public void Setup()
    {
        _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        _purchaseService = new PurchasesService(_purchaseRepositoryMock.Object);
    }

    [Test]
    public async Task GetPurchases_ReturnsPurchases()
    {
        var purchases = PurchasesTests.CreatePurchases();

        _purchaseRepositoryMock.Setup(repo => repo.GetPurchases()).ReturnsAsync(purchases);

        var result = await _purchaseService.GetPurchases();

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(purchases[0].Id));
            Assert.That(result.First().SupplierId, Is.EqualTo(purchases[0].SupplierId));
            Assert.That(result.First().PurchaseDate, Is.EqualTo(purchases[0].PurchaseDate));
            Assert.That(result.First().Total, Is.EqualTo(purchases[0].Total));
            Assert.That(result.Last().Id, Is.EqualTo(purchases[1].Id));
            Assert.That(result.Last().SupplierId, Is.EqualTo(purchases[1].SupplierId));
            Assert.That(result.Last().PurchaseDate, Is.EqualTo(purchases[1].PurchaseDate));
            Assert.That(result.Last().Total, Is.EqualTo(purchases[1].Total));
        });
    }

    [Test]
    public async Task GetPurchaseById_ReturnsPurchase()
    {
        var purchase = PurchasesTests.CreatePurchases().First();

        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseById(purchase.Id)).ReturnsAsync(purchase);

        var result = await _purchaseService.GetPurchaseById(purchase.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(purchase.Id));
            Assert.That(result.SupplierId, Is.EqualTo(purchase.SupplierId));
            Assert.That(result.PurchaseDate, Is.EqualTo(purchase.PurchaseDate));
            Assert.That(result.Total, Is.EqualTo(purchase.Total));
        });
    }

    [Test]
    public void GetPurchaseById_ReturnsNotFound_WhenPurchaseNotFound()
    {
        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseById(It.IsAny<Guid>())).ReturnsAsync((Purchases)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _purchaseService.GetPurchaseById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Purchase not found!"));
        });
    }

    [Test]
    public async Task CreatePurchase_CreatesSuccessfully()
    {
        var purchase = PurchasesTests.CreatePurchases().First();

        _purchaseRepositoryMock.Setup(repo => repo.CreatePurchase(purchase)).ReturnsAsync(purchase);

        var result = await _purchaseService.CreatePurchase(purchase);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(purchase.Id));
            Assert.That(result.SupplierId, Is.EqualTo(purchase.SupplierId));
            Assert.That(result.PurchaseDate, Is.EqualTo(purchase.PurchaseDate));
            Assert.That(result.Total, Is.EqualTo(purchase.Total));
        });
    }

    [Test]
    public async Task UpdatePurchase_UpdatesSucessfully()
    {
        var purchase = PurchasesTests.CreatePurchases().First();
        var updatePurchase = PurchasesTests.UpdatePurchase(purchase.Id, purchase.SupplierId);

        _purchaseRepositoryMock.Setup(repo => repo.CreatePurchase(purchase)).ReturnsAsync(purchase);
        _purchaseRepositoryMock.Setup(repo => repo.UpdatePurchase(purchase)).Returns(Task.CompletedTask);
        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseById(purchase.Id)).ReturnsAsync(purchase);

        await _purchaseService.UpdatePurchase(purchase.Id, updatePurchase);
        var result = await _purchaseService.GetPurchaseById(purchase.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updatePurchase.Id));
            Assert.That(result.SupplierId, Is.EqualTo(updatePurchase.SupplierId));
            Assert.That(result.PurchaseDate, Is.EqualTo(updatePurchase.PurchaseDate));
            Assert.That(result.Total, Is.EqualTo(updatePurchase.Total));
        });
    }

    [Test]
    public async Task DeletePurchase_DeletesSuccessfully()
    {
        var purchase = PurchasesTests.CreatePurchases().First();

        _purchaseRepositoryMock.Setup(repo => repo.CreatePurchase(purchase)).ReturnsAsync(purchase);
        _purchaseRepositoryMock.Setup(repo => repo.DeletePurchase(purchase.Id)).Returns(Task.CompletedTask);
        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseById(purchase.Id)).ReturnsAsync((Purchases)null);

        await _purchaseService.CreatePurchase(purchase);
        await _purchaseService.DeletePurchase(purchase.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _purchaseService.GetPurchaseById(purchase.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Purchase not found!"));
        });
    }
}
