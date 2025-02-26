using Microsoft.Extensions.Configuration;
using StockFlow.Application.Services.Email.Interfaces;
using System.Net;
using System.Net.Mail;

namespace StockFlow.Application.Services.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmail(MailMessage message)
    {
        var smtp = new SmtpClient()
        {
            Host = _configuration["EmailSettings:Host"],
            Port = int.Parse(_configuration["EmailSettings:Port"]),
            EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"])
        };

        await smtp.SendMailAsync(message);
    }
}
