using Microsoft.Extensions.Configuration;
using StockFlow.Application.Services.Email;
using StockFlow.Application.Services.Email.Interfaces;
using System.Net.Mail;
using Moq;

namespace StockFlow.Tests.Services;

[TestFixture]
public class EmailPasswordResetServiceTests
{
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<IEmailSender> _mockEmailSender;
    private EmailPasswordResetService _emailPasswordResetService;

    [SetUp]
    public void Setup()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.SetupGet(c => c["EmailSettings:Username"]).Returns("test@example.com");
        _mockConfiguration.SetupGet(c => c["EmailSettings:Password"]).Returns("password");
        _mockConfiguration.SetupGet(c => c["EmailSettings:Host"]).Returns("smtp.example.com");
        _mockConfiguration.SetupGet(c => c["EmailSettings:Port"]).Returns("587");
        _mockConfiguration.SetupGet(c => c["EmailSettings:EnableSsl"]).Returns("true");

        _mockEmailSender = new Mock<IEmailSender>();
        _emailPasswordResetService = new EmailPasswordResetService(_mockEmailSender.Object, _mockConfiguration.Object);
    }

    [Test]
    public async Task SendPasswordResetEmail_SendEmail()
    {
        var email = "tebecim110@hartaria.com";
        var token = "test-token";

        await _emailPasswordResetService.SendPasswordResetEmail(email, token);

        _mockEmailSender.Verify(x => x.SendEmail(It.Is<MailMessage>(msg =>
            msg.To[0].Address == email &&
            msg.Subject == "Password Recovery" &&
            msg.Body.Contains(token)
        )), Times.Once);
    }
}
