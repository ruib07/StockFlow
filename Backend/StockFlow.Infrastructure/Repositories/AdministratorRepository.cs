using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Administrators> Administrators => _context.Administrators;
    private DbSet<PasswordResetToken> PasswordResetTokens => _context.PasswordResetTokens;

    public AdministratorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Administrators> GetAdminById(Guid adminId)
    {
        return await Administrators.FirstOrDefaultAsync(a => a.Id == adminId);
    }

    public async Task<Administrators> GetAdminByEmail(string email)
    {
        return await Administrators.FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<Administrators> CreateAdmin(Administrators admin)
    {
        await Administrators.AddAsync(admin);
        await _context.SaveChangesAsync();

        return admin;
    }

    public async Task<string> GeneratePasswordResetToken(Guid adminId)
    {
        var token = Guid.NewGuid().ToString();
        var expiryDate = DateTime.UtcNow.AddHours(1);

        var passwordResetToken = new PasswordResetToken()
        {
            Id = Guid.NewGuid(),
            AdminId = adminId,
            Token = token,
            ExpiryDate = expiryDate
        };

        await PasswordResetTokens.AddAsync(passwordResetToken);
        await _context.SaveChangesAsync();

        return token;
    }

    public async Task<PasswordResetToken> GetPasswordResetToken(string token)
    {
        return await PasswordResetTokens.Include(prt => prt.Administrator)
                                        .FirstOrDefaultAsync(prt => prt.Token == token && prt.ExpiryDate > DateTime.UtcNow);
    }

    public async Task RemovePasswordResetToken(PasswordResetToken token)
    {
        PasswordResetTokens.Remove(token);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAdmin(Administrators admin)
    {
        Administrators.Update(admin);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAdmin(Guid adminId)
    {
        var admin = await GetAdminById(adminId);

        Administrators.Remove(admin);
        await _context.SaveChangesAsync();
    }
}
