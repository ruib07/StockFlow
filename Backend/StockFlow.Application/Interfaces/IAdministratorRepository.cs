using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface IAdministratorRepository
{
    Task<Administrators> GetAdminById(Guid adminId);
    Task<Administrators> GetAdminByEmail(string email);
    Task<Administrators> CreateAdmin(Administrators admin);
    Task<string> GeneratePasswordResetToken(Guid adminId); 
    Task<PasswordResetToken> GetPasswordResetToken(string token);
    Task RemovePasswordResetToken(PasswordResetToken token);
    Task UpdateAdmin(Administrators admin);
    Task DeleteAdmin(Guid adminId);
}
