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
public class SuppliersControllerTests
{
    private Mock<ISupplierRepository> _supplierRepositoryMock;
    private SuppliersService _supplierService;
    private SuppliersController _supplierController;

    [SetUp]
    public void Setup()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _supplierService = new SuppliersService(_supplierRepositoryMock.Object);
        _supplierController = new SuppliersController(_supplierService);
    }

    [Test]
    public async Task GetSuppliers_ReturnsOkResult_WithSuppliers()
    {
        var suppliers = SuppliersTests.CreateSuppliers();

        _supplierRepositoryMock.Setup(repo => repo.GetSuppliers()).ReturnsAsync(suppliers);

        var result = await _supplierController.GetSuppliers();
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<Suppliers>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(2));
            Assert.That(response.First().Id, Is.EqualTo(suppliers[0].Id));
            Assert.That(response.First().Name, Is.EqualTo(suppliers[0].Name));
            Assert.That(response.First().Email, Is.EqualTo(suppliers[0].Email));
            Assert.That(response.Last().Id, Is.EqualTo(suppliers[1].Id));
            Assert.That(response.Last().Name, Is.EqualTo(suppliers[1].Name));
            Assert.That(response.Last().Email, Is.EqualTo(suppliers[1].Email));
        });
    }

    [Test]
    public async Task GetSupplierById_ReturnsOkResult_WithSupplier()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();

        _supplierRepositoryMock.Setup(repo => repo.GetSupplierById(supplier.Id)).ReturnsAsync(supplier);

        var result = await _supplierController.GetSupplierById(supplier.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as Suppliers;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(supplier.Id));
            Assert.That(response.Name, Is.EqualTo(supplier.Name));
            Assert.That(response.NIF, Is.EqualTo(supplier.NIF));
            Assert.That(response.PhoneNumber, Is.EqualTo(supplier.PhoneNumber));
            Assert.That(response.Email, Is.EqualTo(supplier.Email));
            Assert.That(response.Address, Is.EqualTo(supplier.Address));
        });
    }

    [Test]
    public async Task CreateSupplier_ReturnsCreatedResult_WhenSupplierIsCreated()
    {
        var newSupplier = SuppliersTests.CreateSuppliers().First();

        _supplierRepositoryMock.Setup(repo => repo.CreateSupplier(newSupplier)).ReturnsAsync(newSupplier);

        var result = await _supplierController.CreateSupplier(newSupplier);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(response.Message, Is.EqualTo("Supplier created successfully."));
            Assert.That(response.Id, Is.EqualTo(newSupplier.Id));
        });
    }

    [Test]
    public async Task UpdateSupplier_ReturnsOkResult_WhenSupplierIsUpdated()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();
        var updatedSupplier = SuppliersTests.UpdateSupplier(supplier.Id, "supplierupdated@email.com");

        _supplierRepositoryMock.Setup(repo => repo.GetSupplierById(supplier.Id)).ReturnsAsync(supplier);
        _supplierRepositoryMock.Setup(repo => repo.UpdateSupplier(It.IsAny<Suppliers>())).Returns(Task.CompletedTask);

        var result = await _supplierController.UpdateSupplier(supplier.Id, updatedSupplier);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Supplier updated successfully."));
        });
    }

    [Test]
    public async Task DeleteSupplier_ReturnsNoContentResult_WhenSupplierIsDeleted()
    {
        var supplier = SuppliersTests.CreateSuppliers().First();

        _supplierRepositoryMock.Setup(repo => repo.GetSupplierById(supplier.Id)).ReturnsAsync(supplier);
        _supplierRepositoryMock.Setup(repo => repo.DeleteSupplier(supplier.Id)).Returns(Task.CompletedTask);

        var result = await _supplierController.DeleteSupplier(supplier.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
