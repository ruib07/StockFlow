using Microsoft.Extensions.Configuration;
using StockFlow.Application.Services.Email.Interfaces;
using System.Net.Mail;

namespace StockFlow.Application.Services.Email;

public class EmailPasswordResetService : IEmailPasswordReset
{
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public EmailPasswordResetService(IEmailSender emailSender, IConfiguration configuration)
    {
        _emailSender = emailSender;
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmail(string email, string token)
    {
        var resetLink = $"http://localhost:3000/change-password?token={token}";
        var fromAddress = new MailAddress(_configuration["EmailSettings:Username"], "StockFlow Support");
        var toAddress = new MailAddress(email);

        using var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = "Password Recovery",
            Body = $"Click on the link to reset your password: {resetLink}"
        };

        await _emailSender.SendEmail(message);
    }
}
