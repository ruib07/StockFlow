using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class SuppliersTests
{
    public static List<Suppliers> CreateSuppliers()
    {
        return new List<Suppliers>()
        {
            new Suppliers()
            {
                Id = Guid.NewGuid(),
                Name = "Supplier1 Test",
                NIF = "123456789",
                PhoneNumber = "916543789",
                Email = "supplier1test@email.com",
                Address = "Supplier1 Test Address"
            },
            new Suppliers()
            {
                Id = Guid.NewGuid(),
                Name = "Supplier2 Test",
                NIF = "987654321",
                PhoneNumber = "965789076",
                Email = "supplier2test@email.com",
                Address = "Supplier2 Test Address"
            }
        };
    }

    public static Suppliers UpdateSupplier(Guid id, string email)
    {
        return new Suppliers()
        {
            Id = id,
            Name = "Supplier Updated",
            NIF = "765432789",
            PhoneNumber = "987654367",
            Email = email,
            Address = "Supplier Updated Address"
        };
    }
}
