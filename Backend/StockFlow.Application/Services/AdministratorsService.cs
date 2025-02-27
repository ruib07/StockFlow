using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Services;

public class AdministratorsService
{
    private readonly IAdministratorRepository _administratorRepository;

    public AdministratorsService(IAdministratorRepository administratorRepository)
    {
        _administratorRepository = administratorRepository;
    }

    public async Task<Administrators> GetAdminById(Guid adminId)
    {
        var admin = await _administratorRepository.GetAdminById(adminId);

        if (admin == null) ErrorHelper.ThrowNotFoundException("Admin not found!");

        return admin;
    }

    public async Task<Administrators> CreateAdmin(Administrators admin)
    {
        var existingAdmin = await _administratorRepository.GetAdminByEmail(admin.Email);

        if (existingAdmin != null) ErrorHelper.ThrowConflictException("Email already in use!");

        if  (string.IsNullOrEmpty(admin.Name) || string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Password))
        {
            ErrorHelper.ThrowBadRequestException("Name, email and password are required!");
        }

        admin.Password = PasswordHasherHelper.HashPassword(admin.Password);

        return await _administratorRepository.CreateAdmin(admin);
    }

    public async Task<string> GeneratePasswordResetToken(string email)
    {
        var admin = await _administratorRepository.GetAdminByEmail(email);

        if (admin != null) return await _administratorRepository.GeneratePasswordResetToken(admin.Id);

        return null;
    }

    public async Task UpdatePassword(string token, string newPassword, string confirmNewPassword)
    {
        if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            ErrorHelper.ThrowBadRequestException("Password fields cannot be empty.");

        if (newPassword != confirmNewPassword) ErrorHelper.ThrowBadRequestException("Passwords do not match.");

        var passwordResetToken = await _administratorRepository.GetPasswordResetToken(token);

        if (passwordResetToken == null) ErrorHelper.ThrowBadRequestException("Invalid or expired token.");

        var admin = passwordResetToken.Administrator;
        admin.Password = PasswordHasherHelper.HashPassword(newPassword);

        await _administratorRepository.UpdateAdmin(admin);
        await _administratorRepository.RemovePasswordResetToken(passwordResetToken);
    }

    public async Task<Administrators> UpdateAdmin(Guid adminId, Administrators updateAdmin)
    {
        var currentAdmin = await GetAdminById(adminId);
        var emailExists = await _administratorRepository.GetAdminByEmail(updateAdmin.Email);

        if (emailExists != null && emailExists.Id != adminId) ErrorHelper.ThrowConflictException("Email already in use!");

        currentAdmin.Name = updateAdmin.Name;
        currentAdmin.Email = emailExists.Email;
        currentAdmin.Password = PasswordHasherHelper.HashPassword(updateAdmin.Password);

        await _administratorRepository.UpdateAdmin(currentAdmin);
        return currentAdmin;
    }

    public async Task DeleteAdmin(Guid adminId)
    {
        await _administratorRepository.DeleteAdmin(adminId);
    }
}
