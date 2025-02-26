using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Customers> Customers => _context.Customers;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Customers>> GetCustomers()
    {
        throw new NotImplementedException();
    }

    public Task<Customers> GetCustomerById(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Customers> GetCustomerByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Customers> CreateCustomer(Customers customer)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCustomer(Customers customer)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCustomer(Guid customerId)
    {
        throw new NotImplementedException();
    }
}
