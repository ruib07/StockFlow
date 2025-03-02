using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class PurchaseRepositoryTests
{
    private PurchaseRepository _purchaseRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _purchaseRepository = new PurchaseRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetPurchases_ReturnsPurchases()
    {
        var purchases = CreatePurchases();
        _context.Purchases.AddRange(purchases);
        await _context.SaveChangesAsync();

        var result = await _purchaseRepository.GetPurchases();

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
        var purchase = CreatePurchases().First();

        await _purchaseRepository.CreatePurchase(purchase);

        var result = await _purchaseRepository.GetPurchaseById(purchase.Id);

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
    public async Task CreatePurchase_CreatesSuccessfully()
    {
        var newPurchase = CreatePurchases().First();

        var result = await _purchaseRepository.CreatePurchase(newPurchase);
        var addedPurchase = await _purchaseRepository.GetPurchaseById(newPurchase.Id);

        Assert.That(addedPurchase, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedPurchase.Id, Is.EqualTo(newPurchase.Id));
            Assert.That(addedPurchase.SupplierId, Is.EqualTo(newPurchase.SupplierId));
            Assert.That(addedPurchase.PurchaseDate, Is.EqualTo(newPurchase.PurchaseDate));
            Assert.That(addedPurchase.Total, Is.EqualTo(newPurchase.Total));
        });
    }

    [Test]
    public async Task UpdatePurchase_UpdatesSuccessfully()
    {
        var existingPurchase = CreatePurchases().First();
        await _purchaseRepository.CreatePurchase(existingPurchase);

        _context.Entry(existingPurchase).State = EntityState.Detached;

        var updatedPurchase = UpdatePurchase(existingPurchase.Id, existingPurchase.SupplierId);

        await _purchaseRepository.UpdatePurchase(updatedPurchase);
        var retrievedUpdatedPurchase = await _purchaseRepository.GetPurchaseById(existingPurchase.Id);

        Assert.That(retrievedUpdatedPurchase, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedPurchase.Id, Is.EqualTo(updatedPurchase.Id));
            Assert.That(retrievedUpdatedPurchase.SupplierId, Is.EqualTo(updatedPurchase.SupplierId));
            Assert.That(retrievedUpdatedPurchase.PurchaseDate, Is.EqualTo(updatedPurchase.PurchaseDate));
            Assert.That(retrievedUpdatedPurchase.Total, Is.EqualTo(updatedPurchase.Total));
        });
    }

    [Test]
    public async Task DeletePurchase_DeletesSuccessfully()
    {
        var existingPurchase = CreatePurchases().First();

        await _purchaseRepository.CreatePurchase(existingPurchase);
        await _purchaseRepository.DeletePurchase(existingPurchase.Id);
        var retrievedEmptyPurchase = await _purchaseRepository.GetPurchaseById(existingPurchase.Id);

        Assert.That(retrievedEmptyPurchase, Is.Null);
    }

    #region Private Methods

    private static List<Purchases> CreatePurchases()
    {
        return new List<Purchases>()
        {
            new Purchases()
            {
                Id = Guid.NewGuid(),
                SupplierId = Guid.NewGuid(),
                PurchaseDate = DateTime.UtcNow,
                Total = 149.99m,
            },
            new Purchases()
            {
                Id = Guid.NewGuid(),
                SupplierId = Guid.NewGuid(),
                PurchaseDate = DateTime.UtcNow,
                Total = 199.99m,
            }
        };
    }

    private static Purchases UpdatePurchase(Guid id, Guid supplierId)
    {
        return new Purchases()
        {
            Id = id,
            SupplierId = supplierId,
            PurchaseDate = DateTime.UtcNow.AddDays(3),
            Total = 99.99m
        };
    }

    #endregion
}
