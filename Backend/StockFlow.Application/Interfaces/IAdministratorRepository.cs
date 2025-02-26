using StockFlow.Domain.Entities;

namespace StockFlow.Application.Interfaces;

public interface IAdministratorRepository
{
    Task<Administrators> GetAdminById(Guid adminId);
    Task<Administrators> GetAdminByEmail(string email);
    Task<Administrators> CreateAdmin(Administrators admin);
    Task UpdateAdmin(Administrators admin);
    Task DeleteAdmin(Guid adminId);
}
