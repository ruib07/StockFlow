using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class SaleRepositoryTests
{
    private SaleRepository _saleRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _saleRepository = new SaleRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetSales_ReturnsSales()
    {
        var sales = CreateSales();
        _context.Sales.AddRange(sales);
        await _context.SaveChangesAsync();

        var result = await _saleRepository.GetSales();

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
        var sale = CreateSales().First();

        await _saleRepository.CreateSale(sale);

        var result = await _saleRepository.GetSaleById(sale.Id);

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
    public async Task CreateSale_CreatesSuccessfully()
    {
        var newSale = CreateSales().First();

        var result = await _saleRepository.CreateSale(newSale);
        var addedSale = await _saleRepository.GetSaleById(newSale.Id);

        Assert.That(addedSale, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedSale.Id, Is.EqualTo(newSale.Id));
            Assert.That(addedSale.CustomerId, Is.EqualTo(newSale.CustomerId));
            Assert.That(addedSale.SaleDate, Is.EqualTo(newSale.SaleDate));
            Assert.That(addedSale.Total, Is.EqualTo(newSale.Total));
        });
    }

    [Test]
    public async Task UpdateSale_UpdatesSuccessfully()
    {
        var existingSale = CreateSales().First();
        await _saleRepository.CreateSale(existingSale);

        _context.Entry(existingSale).State = EntityState.Detached;

        var updatedSale = UpdateSale(existingSale.Id, existingSale.CustomerId);

        await _saleRepository.UpdateSale(updatedSale);
        var retrievedUpdateSale = await _saleRepository.GetSaleById(existingSale.Id);

        Assert.That(retrievedUpdateSale, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdateSale.Id, Is.EqualTo(updatedSale.Id));
            Assert.That(retrievedUpdateSale.CustomerId, Is.EqualTo(updatedSale.CustomerId));
            Assert.That(retrievedUpdateSale.SaleDate, Is.EqualTo(updatedSale.SaleDate));
            Assert.That(retrievedUpdateSale.Total, Is.EqualTo(updatedSale.Total));
        });
    }

    [Test]
    public async Task DeleteSale_DeletesSuccessfully()
    {
        var existingSale = CreateSales().First();

        await _saleRepository.CreateSale(existingSale);
        await _saleRepository.DeleteSale(existingSale.Id);
        var retrievedEmptySale = await _saleRepository.GetSaleById(existingSale.Id);

        Assert.That(retrievedEmptySale, Is.Null);
    }

    #region Private Methods

    private static List<Sales> CreateSales()
    {
        return new List<Sales>()
        {
            new Sales()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Total = 99.99m
            },
            new Sales()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Total = 134.99m
            }
        };
    }

    private static Sales UpdateSale(Guid id, Guid customerId)
    {
        return new Sales()
        {
            Id = id,
            CustomerId = customerId,
            SaleDate = DateTime.UtcNow.AddDays(2),
            Total = 149.99m
        };
    }

    #endregion
}
