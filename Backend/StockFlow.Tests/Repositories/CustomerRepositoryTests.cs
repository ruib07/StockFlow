using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;

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
        var customers = CreateCustomers();
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
        var customer = CreateCustomers().First();

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
        var customer = CreateCustomers().First();

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
        var newCustomer = CreateCustomers().First();

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
        var existingCustomer = CreateCustomers().First();
        await _customerRepository.CreateCustomer(existingCustomer);

        _context.Entry(existingCustomer).State = EntityState.Detached;

        var updatedCustomer = UpdateCustomer(existingCustomer.Id);

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
        var existingCustomer = CreateCustomers().First();

        await _customerRepository.CreateCustomer(existingCustomer);
        await _customerRepository.DeleteCustomer(existingCustomer.Id);
        var retrievedEmptyCustomer = await _customerRepository.GetCustomerById(existingCustomer.Id);

        Assert.That(retrievedEmptyCustomer, Is.Null);
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
                PhoneNumber = "912345678",
                Email = "customer1test@email.com",
                Address = "Customer1 Test address"
            },
            new Customers()
            {
                Id = Guid.NewGuid(),
                Name = "Customer2 Test",
                NIF = "987654321",
                PhoneNumber = "965432178",
                Email = "customer2test@email.com",
                Address = "Customer2 Test address"
            }
        };
    }

    private static Customers UpdateCustomer(Guid id)
    {
        return new Customers()
        {
            Id = id,
            Name = "Customer Updated",
            NIF = "231456543",
            PhoneNumber = "918907654",
            Email = "customerupdated@email.com",
            Address = "Customer Updated address"
        };
    }

    #endregion
}
