using StockFlow.Application.Helpers;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Tests.Templates;

public static class AdministratorsTests
{
    public static Administrators CreateAdmin()
    {
        return new Administrators()
        {
            Id = Guid.NewGuid(),
            Name = "Admin1 Test",
            Email = "admin1test@email.com",
            Password = PasswordHasherHelper.HashPassword("Admin1@Test-123")
        };
    }

    public static Administrators InvalidAdminCreation(string name, string email, string password, decimal balance)
    {
        return new Administrators()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = PasswordHasherHelper.HashPassword(password)
        };
    }

    public static RecoverPasswordDTO.UpdatePassword UpdatePasswordCreation(string token, string newPassword, string confirmNewPassword)
    {
        return new RecoverPasswordDTO.UpdatePassword(token, newPassword, confirmNewPassword);
    }

    public static PasswordResetToken CreateToken(string token, Administrators admin)
    {
        return new PasswordResetToken()
        {
            Id = Guid.NewGuid(),
            Token = token,
            AdminId = admin.Id,
            Administrator = admin,
            ExpiryDate = DateTime.UtcNow
        };
    }

    public static Administrators UpdateAdmin(Guid id, string email)
    {
        return new Administrators()
        {
            Id = id,
            Name = "Admin Updated",
            Email = email,
            Password = PasswordHasherHelper.HashPassword("Admin@Updated-123")
        };
    }
}
