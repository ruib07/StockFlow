using Microsoft.EntityFrameworkCore;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;
using StockFlow.Tests.Templates;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class SupplierRepositoryTests
{
    private SupplierRepository _supplierRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                     .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _supplierRepository = new SupplierRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetSuppliers_ReturnsSuppliers()
    {
        var suppliers = SuppliersTests.CreateSuppliers();
        _context.Suppliers.AddRange(suppliers);
        await _context.SaveChangesAsync();

        var result = await _supplierRepository.GetSuppliers();

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(suppliers[0].Id));
            Assert.That(result.First().Name, Is.EqualTo(suppliers[0].Name));
            Assert.That(result.First().NIF, Is.EqualTo(suppliers[0].NIF));
            Assert.That(result.First().PhoneNumber, Is.EqualTo(suppliers[0].PhoneNumber));
            Assert.That(result.First().Email, Is.EqualTo(suppliers[0].Email));
            Assert.That(result.First().Address, Is.EqualTo(suppliers[0].Address));
            Assert.That(result.Last().Id, Is.EqualTo(suppliers[1].Id));
            Assert.That(result.Last().Name, Is.EqualTo(suppliers[1].Name));
            Assert.That(result.Last().NIF, Is.EqualTo(suppliers[1].NIF));
            Assert.That(result.Last().PhoneNumber, Is.EqualTo(suppliers[1].PhoneNumber));
            Assert.That(result.Last().Email, Is.EqualTo(suppliers[1].Email));
            Assert.That(result.Last().Address, Is.EqualTo(suppliers[1].Address));
        });
    }

    [Test]
    public async Task GetSupplierById_ReturnsSupplier()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();

        await _supplierRepository.CreateSupplier(supplier);

        var result = await _supplierRepository.GetSupplierById(supplier.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(supplier.Id));
            Assert.That(result.Name, Is.EqualTo(supplier.Name));
            Assert.That(result.NIF, Is.EqualTo(supplier.NIF));
            Assert.That(result.PhoneNumber, Is.EqualTo(supplier.PhoneNumber));
            Assert.That(result.Email, Is.EqualTo(supplier.Email));
            Assert.That(result.Address, Is.EqualTo(supplier.Address));
        });
    }

    [Test]
    public async Task GetSupplierByEmail_ReturnsSupplier()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();

        await _supplierRepository.CreateSupplier(supplier);

        var result = await _supplierRepository.GetSupplierByEmail(supplier.Email);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(supplier.Id));
            Assert.That(result.Name, Is.EqualTo(supplier.Name));
            Assert.That(result.NIF, Is.EqualTo(supplier.NIF));
            Assert.That(result.PhoneNumber, Is.EqualTo(supplier.PhoneNumber));
            Assert.That(result.Email, Is.EqualTo(supplier.Email));
            Assert.That(result.Address, Is.EqualTo(supplier.Address));
        });
    }

    [Test]
    public async Task CreateSupplier_CreatesSuccessfully()
    {
        var newSupplier = SuppliersTests.CreateSuppliers().First();

        var result = await _supplierRepository.CreateSupplier(newSupplier);
        var addedSupplier = await _supplierRepository.GetSupplierById(newSupplier.Id);

        Assert.That(addedSupplier, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedSupplier.Id, Is.EqualTo(newSupplier.Id));
            Assert.That(addedSupplier.Name, Is.EqualTo(newSupplier.Name));
            Assert.That(addedSupplier.NIF, Is.EqualTo(newSupplier.NIF));
            Assert.That(addedSupplier.PhoneNumber, Is.EqualTo(newSupplier.PhoneNumber));
            Assert.That(addedSupplier.Email, Is.EqualTo(newSupplier.Email));
            Assert.That(addedSupplier.Address, Is.EqualTo(newSupplier.Address));
        });
    }

    [Test]
    public async Task UpdateSupplier_UpdatesSuccessfully()
    {
        var existingSupplier = SuppliersTests.CreateSuppliers().First();
        await _supplierRepository.CreateSupplier(existingSupplier);

        _context.Entry(existingSupplier).State = EntityState.Detached;

        var updatedSupplier = SuppliersTests.UpdateSupplier(existingSupplier.Id, "supplierupdated@email.com");

        await _supplierRepository.UpdateSupplier(updatedSupplier);
        var retrievedUpdatedSupplier = await _supplierRepository.GetSupplierById(existingSupplier.Id);

        Assert.That(retrievedUpdatedSupplier, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedSupplier.Id, Is.EqualTo(updatedSupplier.Id));
            Assert.That(retrievedUpdatedSupplier.Name, Is.EqualTo(updatedSupplier.Name));
            Assert.That(retrievedUpdatedSupplier.NIF, Is.EqualTo(updatedSupplier.NIF));
            Assert.That(retrievedUpdatedSupplier.PhoneNumber, Is.EqualTo(updatedSupplier.PhoneNumber));
            Assert.That(retrievedUpdatedSupplier.Email, Is.EqualTo(updatedSupplier.Email));
            Assert.That(retrievedUpdatedSupplier.Address, Is.EqualTo(updatedSupplier.Address));
        });
    }

    [Test]
    public async Task DeleteSupplier_DeletesSuccessfully()
    {
        var existingSupplier = SuppliersTests.CreateSuppliers().First();

        await _supplierRepository.CreateSupplier(existingSupplier);
        await _supplierRepository.DeleteSupplier(existingSupplier.Id);
        var retrievedEmptySupplier = await _supplierRepository.GetSupplierById(existingSupplier.Id);

        Assert.That(retrievedEmptySupplier, Is.Null);
    }
}
