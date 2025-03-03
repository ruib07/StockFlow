using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class AdministratorsServiceTests
{
    private Mock<IAdministratorRepository> _administratorRepositoryMock;
    private AdministratorsService _administratorService;

    [SetUp]
    public void Setup()
    {
        _administratorRepositoryMock = new Mock<IAdministratorRepository>();
        _administratorService = new AdministratorsService(_administratorRepositoryMock.Object);
    }

    [Test]
    public async Task GetAdminById_ReturnsAdministrator()
    {
        var admin = CreateAdmin();

        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(admin.Id)).ReturnsAsync(admin);

        var result = await _administratorService.GetAdminById(admin.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(admin.Id));
            Assert.That(result.Name, Is.EqualTo(admin.Name));
            Assert.That(result.Email, Is.EqualTo(admin.Email));
            Assert.That(result.Password, Is.EqualTo(admin.Password));
        });
    }

    [Test]
    public void GetAdminById_ReturnsNotFound_WhenAdminNotFound()
    {
        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(It.IsAny<Guid>())).ReturnsAsync((Administrators)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _administratorService.GetAdminById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Admin not found!"));
        });
    }

    [Test]
    public async Task CreateAdmin_CreatesSuccessfully()
    {
        var admin = CreateAdmin();

        _administratorRepositoryMock.Setup(repo => repo.CreateAdmin(admin)).ReturnsAsync(admin);

        var result = await _administratorService.CreateAdmin(admin);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(admin.Id));
            Assert.That(result.Name, Is.EqualTo(admin.Name));
            Assert.That(result.Email, Is.EqualTo(admin.Email));
            Assert.That(result.Password, Is.EqualTo(admin.Password));
        });
    }

    [Test]
    public void CreateAdmin_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var admin = CreateAdmin();

        _administratorRepositoryMock.Setup(repo => repo.GetAdminByEmail(admin.Email)).ReturnsAsync(admin);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _administratorService.CreateAdmin(admin));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(exception.Message, Is.EqualTo("Email already in use!"));
        });
    }

    [Test]
    public void CreateAdmin_ReturnsBadRequest_WhenNameEmailAndPasswordAreEmpty()
    {
        var invalidAdmin = InvalidAdminCreation(string.Empty, string.Empty, string.Empty, 0);

        _administratorRepositoryMock.Setup(repo => repo.CreateAdmin(invalidAdmin)).ReturnsAsync(invalidAdmin);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _administratorService.CreateAdmin(invalidAdmin));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo("Name, email and password are required!"));
        });
    }

    [Test]
    public async Task GeneratePasswordResetToken_ReturnsToken()
    {
        var admin = CreateAdmin();

        _administratorRepositoryMock.Setup(repo => repo.CreateAdmin(admin)).ReturnsAsync(admin);
        _administratorRepositoryMock.Setup(repo => repo.GetAdminByEmail(admin.Email)).ReturnsAsync(admin);
        _administratorRepositoryMock.Setup(repo => repo.GeneratePasswordResetToken(admin.Id)).ReturnsAsync("token");

        var result = await _administratorService.GeneratePasswordResetToken(admin.Email);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("token"));
    }

    [Test]
    public async Task UpdatePassword_ValidToken_UpdatesPasswordSuccessfully()
    {
        var admin = CreateAdmin();
        var updatePassword = UpdatePasswordCreation("valid-token", "New@Password-123", "New@Password-123");
        var passwordResetToken = new PasswordResetToken() { Token = updatePassword.Token, Administrator = admin };

        _administratorRepositoryMock.Setup(repo => repo.CreateAdmin(admin)).ReturnsAsync(admin);
        _administratorRepositoryMock.Setup(repo => repo.GetPasswordResetToken(updatePassword.Token)).ReturnsAsync(passwordResetToken);
        _administratorRepositoryMock.Setup(repo => repo.UpdateAdmin(It.IsAny<Administrators>())).Returns(Task.CompletedTask);
        _administratorRepositoryMock.Setup(repo => repo.RemovePasswordResetToken(passwordResetToken)).Returns(Task.CompletedTask);
        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(admin.Id)).ReturnsAsync(admin);

        await _administratorService.UpdatePassword(updatePassword.Token, updatePassword.NewPassword, updatePassword.ConfirmNewPassword);
        var result = await _administratorService.GetAdminById(admin.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Password, Is.EqualTo(admin.Password));
    }

    [Test]
    public void UpdatePassword_ThrowsBadRequest_WhenPasswordFieldsAreEmpty()
    {
        var updatePassword = UpdatePasswordCreation("valid-token", string.Empty, string.Empty);

        var exception = Assert.ThrowsAsync<CustomException>(() => _administratorService
                              .UpdatePassword(updatePassword.Token, updatePassword.NewPassword, updatePassword.ConfirmNewPassword));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo("Password fields cannot be empty."));
        });
    }

    [Test]
    public void UpdatePassword_ThrowsBadRequest_WhenPasswordsDoNotMatch()
    {
        var updatePassword = UpdatePasswordCreation("valid-token", "NewPassword123!", "DifferentPassword123!");

        var exception = Assert.ThrowsAsync<CustomException>(() => _administratorService
                              .UpdatePassword(updatePassword.Token, updatePassword.NewPassword, updatePassword.ConfirmNewPassword));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo("Passwords do not match."));
        });
    }

    [Test]
    public void UpdatePassword_ThrowsBadRequest_WhenTokenIsInvalid()
    {
        var updatePassword = UpdatePasswordCreation("invalid-token", "NewPassword123!", "NewPassword123!");

        _administratorRepositoryMock.Setup(repo => repo.GetPasswordResetToken(updatePassword.Token)).ReturnsAsync((PasswordResetToken)null);

        var exception = Assert.ThrowsAsync<CustomException>(() => _administratorService
                              .UpdatePassword(updatePassword.Token, updatePassword.NewPassword, updatePassword.ConfirmNewPassword));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo("Invalid or expired token."));
        });
    }

    [Test]
    public async Task UpdateAdmin_UpdatesSuccessfully()
    {
        var admin = CreateAdmin();
        var updateAdmin = UpdateAdmin(admin.Id, "adminupdated@email.com");

        _administratorRepositoryMock.Setup(repo => repo.CreateAdmin(admin)).ReturnsAsync(admin);
        _administratorRepositoryMock.Setup(repo => repo.UpdateAdmin(updateAdmin)).Returns(Task.CompletedTask);
        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(admin.Id)).ReturnsAsync(admin);

        await _administratorService.UpdateAdmin(admin.Id, updateAdmin);
        var result = await _administratorService.GetAdminById(admin.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateAdmin.Id));
            Assert.That(result.Name, Is.EqualTo(updateAdmin.Name));
            Assert.That(result.Email, Is.EqualTo(updateAdmin.Email));
        });
    }

    [Test]
    public void UpdateAdmin_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var adminId = Guid.NewGuid();
        var existingAdmin = CreateAdmin();
        existingAdmin.Id = adminId;
        existingAdmin.Email = "admin1@email.com";

        var conflictingAdmin = new Administrators() { Id = Guid.NewGuid(), Email = "adminupdated@email.com" };
        var updateAdmin = UpdateAdmin(adminId, "adminupdated@email.com");

        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(adminId)).ReturnsAsync(existingAdmin);
        _administratorRepositoryMock.Setup(repo => repo.GetAdminByEmail(updateAdmin.Email)).ReturnsAsync(conflictingAdmin);

        var exception = Assert.ThrowsAsync<CustomException>(() => _administratorService.UpdateAdmin(adminId, updateAdmin));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(exception.Message, Is.EqualTo("Email already in use!"));
        });
    }

    [Test]
    public async Task DeleteAdmin_DeletesSuccessfully()
    {
        var admin = CreateAdmin();

        _administratorRepositoryMock.Setup(repo => repo.CreateAdmin(admin)).ReturnsAsync(admin);
        _administratorRepositoryMock.Setup(repo => repo.DeleteAdmin(admin.Id)).Returns(Task.CompletedTask);
        _administratorRepositoryMock.Setup(repo => repo.GetAdminById(admin.Id)).ReturnsAsync((Administrators)null);

        await _administratorService.CreateAdmin(admin);
        await _administratorService.DeleteAdmin(admin.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _administratorService.GetAdminById(admin.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Admin not found!"));
        });
    }

    #region Private Method

    private static Administrators CreateAdmin()
    {
        return new Administrators()
        {
            Id = Guid.NewGuid(),
            Name = "Admin1 Test",
            Email = "admin1test@email.com",
            Password = PasswordHasherHelper.HashPassword("Admin1@Test-123")
        };
    }

    private static Administrators InvalidAdminCreation(string name, string email, string password, decimal balance)
    {
        return new Administrators()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = PasswordHasherHelper.HashPassword(password)
        };
    }

    private static RecoverPasswordDTO.UpdatePassword UpdatePasswordCreation(string token, string newPassword, string confirmNewPassword)
    {
        return new RecoverPasswordDTO.UpdatePassword(token, newPassword, confirmNewPassword);
    }

    private static Administrators UpdateAdmin(Guid id, string email)
    {
        return new Administrators()
        {
            Id = id,
            Name = "Admin Updated",
            Email = email,
            Password = PasswordHasherHelper.HashPassword("Admin@Updated-123")
        };
    }

    #endregion
}
