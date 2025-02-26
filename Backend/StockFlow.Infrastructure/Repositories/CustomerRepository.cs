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

    public async Task<IEnumerable<Customers>> GetCustomers()
    {
        return await Customers.ToListAsync();
    }

    public async Task<Customers> GetCustomerById(Guid customerId)
    {
        return await Customers.FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<Customers> GetCustomerByEmail(string email)
    {
        return await Customers.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customers> CreateCustomer(Customers customer)
    {
        await Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task UpdateCustomer(Customers customer)
    {
        Customers.Update(customer);
        await _context.SaveChangesAsync();  
    }

    public async Task DeleteCustomer(Guid customerId)
    {
        var customer = await GetCustomerById(customerId);

        Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}
