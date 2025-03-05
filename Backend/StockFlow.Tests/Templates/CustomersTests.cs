using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class CustomersTests
{
    public static List<Customers> CreateCustomers()
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

    public static Customers UpdateCustomer(Guid id, string email)
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
}
