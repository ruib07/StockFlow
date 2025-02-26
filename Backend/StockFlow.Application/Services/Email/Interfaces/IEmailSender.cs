using System.Net.Mail;

namespace StockFlow.Application.Services.Email.Interfaces;

public interface IEmailSender
{
    Task SendEmail(MailMessage message);
}
