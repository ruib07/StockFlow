using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using StockFlow.Tests.Templates;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class SuppliersServiceTests
{
    private Mock<ISupplierRepository> _supplierRepositoryMock;
    private SuppliersService _supplierService;

    [SetUp]
    public void Setup()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _supplierService = new SuppliersService(_supplierRepositoryMock.Object);
    }

    [Test]
    public async Task GetSuppliers_ReturnsSuppliers()
    {
        var suppliers = SuppliersTests.CreateSuppliers();

        _supplierRepositoryMock.Setup(repo => repo.GetSuppliers()).ReturnsAsync(suppliers);

        var result = await _supplierService.GetSuppliers();

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

        _supplierRepositoryMock.Setup(repo => repo.GetSupplierById(supplier.Id)).ReturnsAsync(supplier);

        var result = await _supplierService.GetSupplierById(supplier.Id);

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
    public void GetSupplierById_ReturnsNotFound_WhenSupplierNotFound()
    {
        _supplierRepositoryMock.Setup(repo => repo.GetSupplierById(It.IsAny<Guid>())).ReturnsAsync((Suppliers)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _supplierService.GetSupplierById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Supplier not found!"));
        });
    }

    [Test]
    public async Task CreateSupplier_CreatesSuccessfully()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();

        _supplierRepositoryMock.Setup(repo => repo.CreateSupplier(supplier)).ReturnsAsync(supplier);

        var result = await _supplierService.CreateSupplier(supplier);

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
    public void CreateSupplier_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();

        _supplierRepositoryMock.Setup(repo => repo.GetSupplierByEmail(supplier.Email)).ReturnsAsync(supplier);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _supplierService.CreateSupplier(supplier));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(exception.Message, Is.EqualTo("Email already in use!"));
        });
    }

    [Test]
    public async Task UpdateSupplier_UpdatesSuccessfully()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();
        var updateSupplier = SuppliersTests.UpdateSupplier(supplier.Id, "supplierupdated@email.com");

        _supplierRepositoryMock.Setup(repo => repo.CreateSupplier(supplier)).ReturnsAsync(supplier);
        _supplierRepositoryMock.Setup(repo => repo.UpdateSupplier(updateSupplier)).Returns(Task.CompletedTask);
        _supplierRepositoryMock.Setup(repo => repo.GetSupplierById(supplier.Id)).ReturnsAsync(supplier);

        await _supplierService.UpdateSupplier(supplier.Id, updateSupplier);
        var result = await _supplierService.GetSupplierById(supplier.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateSupplier.Id));
            Assert.That(result.Name, Is.EqualTo(updateSupplier.Name));
            Assert.That(result.NIF, Is.EqualTo(updateSupplier.NIF));
            Assert.That(result.PhoneNumber, Is.EqualTo(updateSupplier.PhoneNumber));
            Assert.That(result.Email, Is.EqualTo(updateSupplier.Email));
            Assert.That(result.Address, Is.EqualTo(updateSupplier.Address));
        });
    }

    [Test]
    public void UpdateSupplier_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var supplierId = Guid.NewGuid();
        var existingSupplier = SuppliersTests.CreateSuppliers().First();
        existingSupplier.Id = supplierId;
        existingSupplier.Email = "supplier1@email.com";

        var conflictingSupplier = new Suppliers() { Id = Guid.NewGuid(), Email = "supplierupdated@email.com" };
        var updateSupplier = SuppliersTests.UpdateSupplier(supplierId, "supplierupdated@email.com");

        _supplierRepositoryMock.Setup(repo => repo.GetSupplierById(supplierId)).ReturnsAsync(existingSupplier);
        _supplierRepositoryMock.Setup(repo => repo.GetSupplierByEmail(updateSupplier.Email)).ReturnsAsync(conflictingSupplier);

        var exception = Assert.ThrowsAsync<CustomException>(() => _supplierService.UpdateSupplier(supplierId, updateSupplier));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(exception.Message, Is.EqualTo("Email already in use!"));
        });
    }

    [Test]
    public async Task DeleteSupplier_DeletesSuccessfully()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();

        _supplierRepositoryMock.Setup(repo => repo.CreateSupplier(supplier)).ReturnsAsync(supplier);
        _supplierRepositoryMock.Setup(repo => repo.DeleteSupplier(supplier.Id)).Returns(Task.CompletedTask);
        _supplierRepositoryMock.Setup(repo => repo.GetSupplierById(supplier.Id)).ReturnsAsync((Suppliers)null);

        await _supplierService.CreateSupplier(supplier);
        await _supplierService.DeleteSupplier(supplier.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _supplierService.GetSupplierById(supplier.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Supplier not found!"));
        });
    }
}
