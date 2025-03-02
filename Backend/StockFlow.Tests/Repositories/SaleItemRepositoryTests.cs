using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class SaleItemRepositoryTests
{
    private SaleItemRepository _saleItemRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _saleItemRepository = new SaleItemRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetSaleItemById_ReturnsSaleItem()
    {
        var saleItem = CreateSaleItems().First();

        await _saleItemRepository.CreateSaleItem(saleItem);

        var result = await _saleItemRepository.GetSaleItemById(saleItem.Id);

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
    public async Task GetSaleItemsBySaleId_ReturnsSaleItems()
    {
        var saleItems = CreateSaleItems();
        _context.SaleItems.AddRange(saleItems);
        await _context.SaveChangesAsync();

        var result = await _saleItemRepository.GetSaleItemsBySaleId(saleItems[0].SaleId);

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
    public async Task CreateSaleItem_CreatesSuccessfully()
    {
        var newSaleItem = CreateSaleItems().First();

        var result = await _saleItemRepository.CreateSaleItem(newSaleItem);
        var addedSaleItem = await _saleItemRepository.GetSaleItemById(newSaleItem.Id);

        Assert.That(addedSaleItem, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedSaleItem.Id, Is.EqualTo(newSaleItem.Id));
            Assert.That(addedSaleItem.SaleId, Is.EqualTo(newSaleItem.SaleId));
            Assert.That(addedSaleItem.ProductId, Is.EqualTo(newSaleItem.ProductId));
            Assert.That(addedSaleItem.Quantity, Is.EqualTo(newSaleItem.Quantity));
            Assert.That(addedSaleItem.UnitPrice, Is.EqualTo(newSaleItem.UnitPrice));
            Assert.That(addedSaleItem.SubTotal, Is.EqualTo(newSaleItem.SubTotal));
        });
    }

    [Test]
    public async Task UpdateSaleItem_UpdatesSuccessfully()
    {
        var existingSaleItem = CreateSaleItems().First();
        await _saleItemRepository.CreateSaleItem(existingSaleItem);

        _context.Entry(existingSaleItem).State = EntityState.Detached;

        var updatedSaleItem = UpdateSaleItem(existingSaleItem.Id, existingSaleItem.SaleId, existingSaleItem.ProductId);

        await _saleItemRepository.UpdateSaleItem(updatedSaleItem);
        var retrievedUpdatedSaleItem = await _saleItemRepository.GetSaleItemById(existingSaleItem.Id);

        Assert.That(retrievedUpdatedSaleItem, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedSaleItem.Id, Is.EqualTo(updatedSaleItem.Id));
            Assert.That(retrievedUpdatedSaleItem.SaleId, Is.EqualTo(updatedSaleItem.SaleId));
            Assert.That(retrievedUpdatedSaleItem.ProductId, Is.EqualTo(updatedSaleItem.ProductId));
            Assert.That(retrievedUpdatedSaleItem.Quantity, Is.EqualTo(updatedSaleItem.Quantity));
            Assert.That(retrievedUpdatedSaleItem.UnitPrice, Is.EqualTo(updatedSaleItem.UnitPrice));
            Assert.That(retrievedUpdatedSaleItem.SubTotal, Is.EqualTo(updatedSaleItem.SubTotal));
        });
    }

    [Test]
    public async Task DeleteSaleItem_DeletesSuccessfully()
    {
        var existingSaleItem = CreateSaleItems().First();

        await _saleItemRepository.CreateSaleItem(existingSaleItem);
        await _saleItemRepository.DeleteSaleItem(existingSaleItem.Id);
        var retrievedEmptySaleItem = await _saleItemRepository.GetSaleItemById(existingSaleItem.Id);

        Assert.That(retrievedEmptySaleItem, Is.Null);
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
            UnitPrice = 59.99m,
            SubTotal = 119.98m
        };
    }

    #endregion
}
