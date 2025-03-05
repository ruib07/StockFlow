using Microsoft.EntityFrameworkCore;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;
using StockFlow.Tests.Templates;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class PurchaseItemRepositoryTests
{
    private PurchaseItemRepository _purchaseItemRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _purchaseItemRepository = new PurchaseItemRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetPurchaseItemById_ReturnsPurchaseItem()
    {
        var purchaseItem = PurchaseItemsTests.CreatePurchaseItems().First();

        await _purchaseItemRepository.CreatePurchaseItem(purchaseItem);

        var result = await _purchaseItemRepository.GetPurchaseItemById(purchaseItem.Id);

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
    public async Task GetPurchaseItemsByPurchaseId_ReturnsPurchaseItem()
    {
        var purchaseItems = PurchaseItemsTests.CreatePurchaseItems();
        _context.PurchaseItems.AddRange(purchaseItems);
        await _context.SaveChangesAsync();

        var result = await _purchaseItemRepository.GetPurchaseItemsByPurchaseId(purchaseItems[0].PurchaseId);

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
    public async Task CreatePurchaseItem_CreatesSuccessfully()
    {
        var newPurchaseItem = PurchaseItemsTests.CreatePurchaseItems().First();

        var result = await _purchaseItemRepository.CreatePurchaseItem(newPurchaseItem);
        var addedPurchaseItem = await _purchaseItemRepository.GetPurchaseItemById(newPurchaseItem.Id);

        Assert.That(addedPurchaseItem, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedPurchaseItem.Id, Is.EqualTo(newPurchaseItem.Id));
            Assert.That(addedPurchaseItem.PurchaseId, Is.EqualTo(newPurchaseItem.PurchaseId));
            Assert.That(addedPurchaseItem.ProductId, Is.EqualTo(newPurchaseItem.ProductId));
            Assert.That(addedPurchaseItem.Quantity, Is.EqualTo(newPurchaseItem.Quantity));
            Assert.That(addedPurchaseItem.UnitPrice, Is.EqualTo(newPurchaseItem.UnitPrice));
            Assert.That(addedPurchaseItem.SubTotal, Is.EqualTo(newPurchaseItem.SubTotal));
        });
    }

    [Test]
    public async Task UpdatePurchaseItem_UpdatesSuccessfully()
    {
        var existingPurchaseItem = PurchaseItemsTests.CreatePurchaseItems().First();
        await _purchaseItemRepository.CreatePurchaseItem(existingPurchaseItem);

        _context.Entry(existingPurchaseItem).State = EntityState.Detached;

        var updatedPurchaseItem = PurchaseItemsTests.UpdatePurchaseItem(existingPurchaseItem.Id, existingPurchaseItem.PurchaseId, existingPurchaseItem.ProductId);

        await _purchaseItemRepository.UpdatePurchaseItem(updatedPurchaseItem);
        var retrieveUpdatedPurchaseItem = await _purchaseItemRepository.GetPurchaseItemById(existingPurchaseItem.Id);

        Assert.That(retrieveUpdatedPurchaseItem, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrieveUpdatedPurchaseItem.Id, Is.EqualTo(updatedPurchaseItem.Id));
            Assert.That(retrieveUpdatedPurchaseItem.PurchaseId, Is.EqualTo(updatedPurchaseItem.PurchaseId));
            Assert.That(retrieveUpdatedPurchaseItem.ProductId, Is.EqualTo(updatedPurchaseItem.ProductId));
            Assert.That(retrieveUpdatedPurchaseItem.Quantity, Is.EqualTo(updatedPurchaseItem.Quantity));
            Assert.That(retrieveUpdatedPurchaseItem.UnitPrice, Is.EqualTo(updatedPurchaseItem.UnitPrice));
            Assert.That(retrieveUpdatedPurchaseItem.SubTotal, Is.EqualTo(updatedPurchaseItem.SubTotal));
        });
    }

    [Test]
    public async Task DeletePurchaseItem_DeletesSuccessfully()
    {
        var existingPurchaseItem = PurchaseItemsTests.CreatePurchaseItems().First();

        await _purchaseItemRepository.CreatePurchaseItem(existingPurchaseItem);
        await _purchaseItemRepository.DeletePurchaseItem(existingPurchaseItem.Id);
        var retrievedEmptyPurchaseItem = await _purchaseItemRepository.GetPurchaseItemById(existingPurchaseItem.Id);

        Assert.That(retrievedEmptyPurchaseItem, Is.Null);
    }
}
