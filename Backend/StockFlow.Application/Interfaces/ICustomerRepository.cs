using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customers>> GetCustomers();
    Task<Customers> GetCustomerById(Guid customerId);
    Task<Customers> GetCustomerByEmail(string email);
    Task<Customers> CreateCustomer(Customers customer);
    Task UpdateCustomer(Customers customer);
    Task DeleteCustomer(Guid customerId);
}
