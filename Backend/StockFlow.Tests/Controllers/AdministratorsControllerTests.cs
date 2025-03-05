using Moq;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Application.Services.Email.Interfaces;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;
using StockFlow.Server.Controllers;
using StockFlow.Tests.Templates;

namespace StockFlow.Tests.Controllers;

[TestFixture]
public class AdministratorsControllerTests
{
    private Mock<IAdministratorRepository> _administratorRepositoryMock;
    private AdministratorsService _administratorsService;
    private Mock<IEmailPasswordReset> _emailServiceMock;
    private AdministratorsController _administratorsController;

    [SetUp]
    public void Setup()
    {
        _administratorRepositoryMock = new Mock<IAdministratorRepository>();
        _administratorsService = new AdministratorsService(_administratorRepositoryMock.Object);
        _emailServiceMock = new Mock<IEmailPasswordReset>();
        _administratorsController = new AdministratorsController(_administratorsService, _emailServiceMock.Object);
    }

    [Test]
    public async Task GetAdminById_ReturnsOkResult_WithAdmin()
    {
        var admin = AdministratorsTests.CreateAdmin();

        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(admin.Id)).ReturnsAsync(admin);

        var result = await _administratorsController.GetAdminById(admin.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as Administrators;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(admin.Id));
            Assert.That(response.Name, Is.EqualTo(admin.Name));
            Assert.That(response.Email, Is.EqualTo(admin.Email));
            Assert.That(response.Password, Is.EqualTo(admin.Password));
        });
    }

    [Test]
    public async Task RecoverPassword_ReturnsOk_WhenTokenIsGeneratedAndEmailIsSent()
    {
        var admin = AdministratorsTests.CreateAdmin();
        var passwordResetToken = "generated_token";

        _administratorRepositoryMock.Setup(repo => repo.GetAdminByEmail(admin.Email)).ReturnsAsync(admin);
        _administratorRepositoryMock.Setup(repo => repo.GeneratePasswordResetToken(admin.Id)).ReturnsAsync(passwordResetToken);
        _emailServiceMock.Setup(repo => repo.SendPasswordResetEmail(admin.Email, passwordResetToken)).Returns(Task.CompletedTask);

        var recoverPasswordRequest = new RecoverPasswordDTO.Request(admin.Email);
        var recoverPasswordEmailResult = await _administratorsController.RecoverPasswordSendEmail(recoverPasswordRequest);
        var recoverPasswordResponse = recoverPasswordEmailResult as OkResult;

        Assert.That(recoverPasswordResponse.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task RecoverPassword_ReturnsOk_WhenInvalidEmail()
    {
        var request = new RecoverPasswordDTO.Request("testuser@gmail.com");
        var token = "";

        _administratorRepositoryMock.Setup(repo => repo.GetAdminByEmail(request.Email)).ReturnsAsync((Administrators)null);
        _emailServiceMock.Setup(repo => repo.SendPasswordResetEmail(request.Email, token)).Returns(Task.CompletedTask);

        var result = await _administratorsController.RecoverPasswordSendEmail(request);
        var okResult = result as OkResult;

        Assert.That(okResult.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task UpdatePassword_ReturnsOkResult_WhenPasswordIsUpdatedSuccessfully()
    {
        var request = AdministratorsTests.UpdatePasswordCreation("validToken", "New@Password-123", "New@Password-123");
        var admin = AdministratorsTests.CreateAdmin();
        var token = AdministratorsTests.CreateToken(request.Token, admin);

        _administratorRepositoryMock.Setup(repo => repo.GetPasswordResetToken(request.Token)).ReturnsAsync(token);
        _administratorRepositoryMock.Setup(repo => repo.UpdateAdmin(It.IsAny<Administrators>())).Returns(Task.CompletedTask);
        _administratorRepositoryMock.Setup(repo => repo.RemovePasswordResetToken(token)).Returns(Task.CompletedTask);

        var result = await _administratorsController.UpdatePassword(request);
        var okResult = result as OkResult;

        Assert.That(okResult.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task UpdateAdmin_ReturnsOkResult_WhenAdminIsUpdated()
    {
        var admin = AdministratorsTests.CreateAdmin();
        var updatedAdmin = AdministratorsTests.UpdateAdmin(admin.Id, "adminupdated@email.com");

        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(admin.Id)).ReturnsAsync(admin);
        _administratorRepositoryMock.Setup(repo => repo.UpdateAdmin(It.IsAny<Administrators>())).Returns(Task.CompletedTask);

        var result = await _administratorsController.UpdateAdmin(admin.Id, updatedAdmin);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Administrator updated successfully."));
        });
    }

    [Test]
    public async Task DeleteAdmin_ReturnsNoContent_WhenAdminIsDeleted()
    {
        var admin = AdministratorsTests.CreateAdmin();

        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(admin.Id)).ReturnsAsync(admin);
        _administratorRepositoryMock.Setup(repo => repo.DeleteAdmin(admin.Id)).Returns(Task.CompletedTask);

        var result = await _administratorsController.DeleteAdmin(admin.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
