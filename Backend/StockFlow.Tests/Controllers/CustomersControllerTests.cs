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
public class CustomersControllerTests
{
    private Mock<ICustomerRepository> _customerRepositoryMock;
    private CustomersService _customersService;
    private CustomersController _customersController;

    [SetUp]
    public void Setup()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customersService = new CustomersService(_customerRepositoryMock.Object);
        _customersController = new CustomersController(_customersService);
    }

    [Test]
    public async Task GetCustomers_ReturnsOkResult_WithCustomers()
    {
        var customers = CustomersTests.CreateCustomers();

        _customerRepositoryMock.Setup(repo => repo.GetCustomers()).ReturnsAsync(customers);

        var result = await _customersController.GetCustomers();
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<Customers>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(2));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.First().Id, Is.EqualTo(customers[0].Id));
            Assert.That(response.First().Name, Is.EqualTo(customers[0].Name));
            Assert.That(response.Last().Id, Is.EqualTo(customers[1].Id));
            Assert.That(response.Last().Name, Is.EqualTo(customers[1].Name));
        });
    }

    [Test]
    public async Task GetCustomerById_ReturnsOkResult_WithCustomer()
    {
        var customer = CustomersTests.CreateCustomers().First();

        _customerRepositoryMock.Setup(repo => repo.GetCustomerById(customer.Id)).ReturnsAsync(customer);

        var result = await _customersController.GetCustomerById(customer.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as Customers;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(customer.Id));
            Assert.That(response.Name, Is.EqualTo(customer.Name));
            Assert.That(response.NIF, Is.EqualTo(customer.NIF));
            Assert.That(response.PhoneNumber, Is.EqualTo(customer.PhoneNumber));
            Assert.That(response.Email, Is.EqualTo(customer.Email));
            Assert.That(response.Address, Is.EqualTo(customer.Address));
        });
    }

    [Test]
    public async Task CreateCustomer_ReturnsCreatedResult_WhenCustomerIsCreated()
    {
        var newCustomer = CustomersTests.CreateCustomers().First();

        _customerRepositoryMock.Setup(repo => repo.CreateCustomer(It.IsAny<Customers>())).ReturnsAsync(newCustomer);

        var result = await _customersController.CreateCustomer(newCustomer);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(response.Message, Is.EqualTo("Customer created successfully."));
            Assert.That(response.Id, Is.EqualTo(newCustomer.Id));
        });
    }

    [Test]
    public async Task UpdateCustomer_ReturnsOkResult_WhenCustomerIsUpdated()
    {
        var customer = CustomersTests.CreateCustomers().First();
        var updatedCustomer = CustomersTests.UpdateCustomer(customer.Id, "customerupdated@email.com");

        _customerRepositoryMock.Setup(repo => repo.GetCustomerById(customer.Id)).ReturnsAsync(customer);
        _customerRepositoryMock.Setup(repo => repo.UpdateCustomer(It.IsAny<Customers>())).Returns(Task.CompletedTask);

        var result = await _customersController.UpdateCustomer(customer.Id, updatedCustomer);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Customer updated successfully."));
        });
    }

    [Test]
    public async Task DeleteCustomer_ReturnsNoContentResult_WhenCustomerIsDeleted()
    {
        var customer = CustomersTests.CreateCustomers().First();

        _customerRepositoryMock.Setup(repo => repo.GetCustomerById(customer.Id)).ReturnsAsync(customer);
        _customerRepositoryMock.Setup(repo => repo.DeleteCustomer(customer.Id)).Returns(Task.CompletedTask);

        var result = await _customersController.DeleteCustomer(customer.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
