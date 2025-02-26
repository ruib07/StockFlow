namespace StockFlow.Application.Services.Email.Interfaces;

public interface IEmailPasswordReset
{
    Task SendPasswordResetEmail(string email, string token);
}
