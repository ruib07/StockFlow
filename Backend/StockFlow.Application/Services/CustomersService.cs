using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class CustomersService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customers>> GetCustomers()
    {
        return await _customerRepository.GetCustomers();
    }

    public async Task<Customers> GetCustomerById(Guid customerId)
    {
        var customer = await _customerRepository.GetCustomerById(customerId);

        if (customer == null) ErrorHelper.ThrowNotFoundException("Customer not found!");

        return customer;
    }

    public async Task<Customers> CreateCustomer(Customers customer)
    {
        var existingCustomer = await _customerRepository.GetCustomerByEmail(customer.Email);

        if (existingCustomer != null) ErrorHelper.ThrowConflictException("Email already in use!");

        return await _customerRepository.CreateCustomer(customer);
    }

    public async Task<Customers> UpdateCustomer(Guid customerId, Customers updateCustomer)
    {
        var currentCustomer = await GetCustomerById(customerId);
        var emailExists = await _customerRepository.GetCustomerByEmail(updateCustomer.Email);

        if (emailExists != null && emailExists.Id != customerId) ErrorHelper.ThrowConflictException("Email already in use!");

        currentCustomer.Name = updateCustomer.Name;
        currentCustomer.NIF = updateCustomer.NIF;
        currentCustomer.PhoneNumber = updateCustomer.PhoneNumber;
        currentCustomer.Email = updateCustomer.Email;
        currentCustomer.Address = updateCustomer.Address;

        await _customerRepository.UpdateCustomer(currentCustomer);
        return currentCustomer;
    }

    public async Task DeleteCustomer(Guid customerId)
    {
        await _customerRepository.DeleteCustomer(customerId);
    }
}
