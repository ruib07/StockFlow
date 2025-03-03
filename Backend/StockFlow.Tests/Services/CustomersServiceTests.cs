using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class CustomersServiceTests
{
    private Mock<ICustomerRepository> _customerRepositoryMock;
    private CustomersService _customersService;

    [SetUp]
    public void Setup()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customersService = new CustomersService(_customerRepositoryMock.Object);
    }

    [Test]
    public async Task GetCustomers_ReturnsCustomers()
    {
        var customers = CreateCustomers();

        _customerRepositoryMock.Setup(repo => repo.GetCustomers()).ReturnsAsync(customers);

        var result = await _customersService.GetCustomers();

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(customers[0].Id));
            Assert.That(result.First().Name, Is.EqualTo(customers[0].Name));
            Assert.That(result.First().NIF, Is.EqualTo(customers[0].NIF));
            Assert.That(result.First().PhoneNumber, Is.EqualTo(customers[0].PhoneNumber));
            Assert.That(result.First().Email, Is.EqualTo(customers[0].Email));
            Assert.That(result.First().Address, Is.EqualTo(customers[0].Address));
            Assert.That(result.Last().Id, Is.EqualTo(customers[1].Id));
            Assert.That(result.Last().Name, Is.EqualTo(customers[1].Name));
            Assert.That(result.Last().NIF, Is.EqualTo(customers[1].NIF));
            Assert.That(result.Last().PhoneNumber, Is.EqualTo(customers[1].PhoneNumber));
            Assert.That(result.Last().Email, Is.EqualTo(customers[1].Email));
            Assert.That(result.Last().Address, Is.EqualTo(customers[1].Address));
        });
    }

    [Test]
    public async Task GetCustomerById_ReturnsCustomer()
    {
        var customer = CreateCustomers().First();

        _customerRepositoryMock.Setup(repo => repo.GetCustomerById(customer.Id)).ReturnsAsync(customer);

        var result = await _customersService.GetCustomerById(customer.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(customer.Id));
            Assert.That(result.Name, Is.EqualTo(customer.Name));
            Assert.That(result.NIF, Is.EqualTo(customer.NIF));
            Assert.That(result.PhoneNumber, Is.EqualTo(customer.PhoneNumber));
            Assert.That(result.Email, Is.EqualTo(customer.Email));
            Assert.That(result.Address, Is.EqualTo(customer.Address));
        });
    }

    [Test]
    public void GetCustomerById_ReturnsNotFound_WhenCustomerNotFound()
    {
        _customerRepositoryMock.Setup(repo => repo.GetCustomerById(It.IsAny<Guid>())).ReturnsAsync((Customers)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _customersService.GetCustomerById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Customer not found!"));
        });
    }

    [Test]
    public async Task CreateCustomer_CreatesSuccessfully()
    {
        var customer = CreateCustomers().First();

        _customerRepositoryMock.Setup(repo => repo.CreateCustomer(customer)).ReturnsAsync(customer);

        var result = await _customersService.CreateCustomer(customer);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(customer.Id));
            Assert.That(result.Name, Is.EqualTo(customer.Name));
            Assert.That(result.NIF, Is.EqualTo(customer.NIF));
            Assert.That(result.PhoneNumber, Is.EqualTo(customer.PhoneNumber));
            Assert.That(result.Email, Is.EqualTo(customer.Email));
            Assert.That(result.Address, Is.EqualTo(customer.Address));
        });
    }

    [Test]
    public void CreateCustomer_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var customer = CreateCustomers().First();

        _customerRepositoryMock.Setup(repo => repo.GetCustomerByEmail(customer.Email)).ReturnsAsync(customer);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _customersService.CreateCustomer(customer));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(exception.Message, Is.EqualTo("Email already in use!"));
        });
    }

    [Test]
    public async Task UpdateCustomer_UpdatesSuccessfully()
    {
        var customer = CreateCustomers().First();
        var updateCustomer = UpdateCustomer(customer.Id, "customerupdated@email.com");

        _customerRepositoryMock.Setup(repo => repo.CreateCustomer(customer)).ReturnsAsync(customer);
        _customerRepositoryMock.Setup(repo => repo.UpdateCustomer(updateCustomer)).Returns(Task.CompletedTask);
        _customerRepositoryMock.Setup(repo => repo.GetCustomerById(customer.Id)).ReturnsAsync(customer);

        await _customersService.UpdateCustomer(customer.Id, updateCustomer);
        var result = await _customersService.GetCustomerById(customer.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateCustomer.Id));
            Assert.That(result.Name, Is.EqualTo(updateCustomer.Name));
            Assert.That(result.NIF, Is.EqualTo(updateCustomer.NIF));
            Assert.That(result.PhoneNumber, Is.EqualTo(updateCustomer.PhoneNumber));
            Assert.That(result.Email, Is.EqualTo(updateCustomer.Email));
            Assert.That(result.Address, Is.EqualTo(updateCustomer.Address));
        });
    }

    [Test]
    public void UpdateCustomer_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var customerId = Guid.NewGuid();
        var existingCustomer = CreateCustomers().First();
        existingCustomer.Id = customerId;
        existingCustomer.Email = "customer1@email.com";

        var conflictingCustomer = new Customers() { Id = Guid.NewGuid(), Email = "customerupdated@email.com" };
        var updateCustomer = UpdateCustomer(customerId, "customerupdated@email.com");

        _customerRepositoryMock.Setup(repo => repo.GetCustomerById(customerId)).ReturnsAsync(existingCustomer);
        _customerRepositoryMock.Setup(repo => repo.GetCustomerByEmail(updateCustomer.Email)).ReturnsAsync(conflictingCustomer);

        var exception = Assert.ThrowsAsync<CustomException>(() => _customersService.UpdateCustomer(customerId, updateCustomer));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(exception.Message, Is.EqualTo("Email already in use!"));
        });
    }

    [Test]
    public async Task DeleteCustomer_DeletesSuccessfully()
    {
        var customer = CreateCustomers().First();

        _customerRepositoryMock.Setup(repo => repo.CreateCustomer(customer)).ReturnsAsync(customer);
        _customerRepositoryMock.Setup(repo => repo.DeleteCustomer(customer.Id)).Returns(Task.CompletedTask);
        _customerRepositoryMock.Setup(repo => repo.GetCustomerById(customer.Id)).ReturnsAsync((Customers)null);

        await _customersService.CreateCustomer(customer);
        await _customersService.DeleteCustomer(customer.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _customersService.GetCustomerById(customer.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Customer not found!"));
        });
    }

    #region Private Methods

    private static List<Customers> CreateCustomers()
    {
        return new List<Customers>()
        {
            new Customers()
            {
                Id = Guid.NewGuid(),
                Name = "Customer1 Test",
                NIF = "123456789",
                PhoneNumber = "918765432",
                Email = "customer1test@email.com",
                Address = "Customer1 Address"
            },
            new Customers()
            {
                Id = Guid.NewGuid(),
                Name = "Customer2 Test",
                NIF = "987654321",
                PhoneNumber = "965432178",
                Email = "customer2test@email.com",
                Address = "Customer2 Address"
            }
        };
    }

    private static Customers UpdateCustomer(Guid id, string email)
    {
        return new Customers()
        {
            Id = id,
            Name = "Customer Updated",
            NIF = "453215678",
            PhoneNumber = "965432878",
            Email = email,
            Address = "Customer Updated Address"
        };
    }

    #endregion
}
