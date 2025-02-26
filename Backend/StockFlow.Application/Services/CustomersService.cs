using StockFlow.Application.Interfaces;

namespace StockFlow.Application.Services;

public class CustomersService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
}
