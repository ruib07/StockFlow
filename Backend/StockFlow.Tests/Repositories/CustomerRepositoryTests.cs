using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;
using StockFlow.Tests.Templates;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class CustomerRepositoryTests
{
    private CustomerRepository _customerRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _customerRepository = new CustomerRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetCustomers_ReturnsCustomers()
    {
        var customers = CustomersTests.CreateCustomers();
        _context.Customers.AddRange(customers);
        await _context.SaveChangesAsync();

        var result = await _customerRepository.GetCustomers();

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(customers[0].Id));    
            Assert.That(result.First().Name, Is.EqualTo(customers[0].Name));
            Assert.That(result.Last().Id, Is.EqualTo(customers[1].Id));
            Assert.That(result.Last().Name, Is.EqualTo(customers[1].Name));
        });
    }

    [Test]
    public async Task GetCustomerById_ReturnsCustomer()
    {
        var customer = CustomersTests.CreateCustomers().First();

        await _customerRepository.CreateCustomer(customer);

        var result = await _customerRepository.GetCustomerById(customer.Id);

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
    public async Task GetCustomerByEmail_ReturnsCustomer()
    {
        var customer = CustomersTests.CreateCustomers().First();

        await _customerRepository.CreateCustomer(customer);

        var result = await _customerRepository.GetCustomerByEmail(customer.Email);

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
    public async Task CreateCustomer_CreatesSuccessfully()
    {
        var newCustomer = CustomersTests.CreateCustomers().First();

        var result = await _customerRepository.CreateCustomer(newCustomer);
        var addedCustomer = await _customerRepository.GetCustomerById(newCustomer.Id);

        Assert.That(addedCustomer, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedCustomer.Id, Is.EqualTo(newCustomer.Id));
            Assert.That(addedCustomer.Name, Is.EqualTo(newCustomer.Name));
            Assert.That(addedCustomer.NIF, Is.EqualTo(newCustomer.NIF));
            Assert.That(addedCustomer.PhoneNumber, Is.EqualTo(newCustomer.PhoneNumber));
            Assert.That(addedCustomer.Email, Is.EqualTo(newCustomer.Email));
            Assert.That(addedCustomer.Address, Is.EqualTo(newCustomer.Address));
        });
    }

    [Test]
    public async Task UpdateCustomer_UpdatesSuccessfully()
    {
        var existingCustomer = CustomersTests.CreateCustomers().First();
        await _customerRepository.CreateCustomer(existingCustomer);

        _context.Entry(existingCustomer).State = EntityState.Detached;

        var updatedCustomer = CustomersTests.UpdateCustomer(existingCustomer.Id, "customerupdated@email.com");

        await _customerRepository.UpdateCustomer(updatedCustomer);
        var retrievedUpdatedCustomer = await _customerRepository.GetCustomerById(existingCustomer.Id);

        Assert.That(retrievedUpdatedCustomer, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedCustomer.Id, Is.EqualTo(updatedCustomer.Id));
            Assert.That(retrievedUpdatedCustomer.Name, Is.EqualTo(updatedCustomer.Name));
            Assert.That(retrievedUpdatedCustomer.NIF, Is.EqualTo(updatedCustomer.NIF));
            Assert.That(retrievedUpdatedCustomer.PhoneNumber, Is.EqualTo(updatedCustomer.PhoneNumber));
            Assert.That(retrievedUpdatedCustomer.Email, Is.EqualTo(updatedCustomer.Email));
            Assert.That(retrievedUpdatedCustomer.Address, Is.EqualTo(updatedCustomer.Address));
        });
    }

    [Test]
    public async Task DeleteCustomer_DeletesSuccessfully()
    {
        var existingCustomer = CustomersTests.CreateCustomers().First();

        await _customerRepository.CreateCustomer(existingCustomer);
        await _customerRepository.DeleteCustomer(existingCustomer.Id);
        var retrievedEmptyCustomer = await _customerRepository.GetCustomerById(existingCustomer.Id);

        Assert.That(retrievedEmptyCustomer, Is.Null);
    }
}
