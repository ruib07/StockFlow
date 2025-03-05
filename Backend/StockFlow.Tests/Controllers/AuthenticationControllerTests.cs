using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using StockFlow.Server.Controllers;
using StockFlow.Tests.Templates;
using System.Security.Cryptography;

namespace StockFlow.Tests.Controllers;

[TestFixture]
public class AuthenticationControllerTests
{
    private ApplicationDbContext _context;
    private JwtDTO _jwt;
    private Mock<IAdministratorRepository> _administratorRepositoryMock;
    private AdministratorsService _administratorsService;
    private AuthenticationController _authenticationController;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        var key = GenerateRandomKey();
        _jwt = new JwtDTO("testIssuer", "testAudience", key);
        _administratorRepositoryMock = new Mock<IAdministratorRepository>();
        _administratorsService = new AdministratorsService(_administratorRepositoryMock.Object);
        _authenticationController = new AuthenticationController(_context, _administratorsService, _jwt);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task Signin_ReturnsOk_WhenCredentialsAreValid()
    {
        var admin = AdministratorsTests.CreateAdmin();
        _context.Administrators.Add(admin);
        await _context.SaveChangesAsync();

        var signinRequest = new LoginDTO.Request(admin.Email, "Admin1@Test-123");

        var result = await _authenticationController.Signin(signinRequest);
        var okResult = result as OkObjectResult;
        var signinResponse = okResult.Value as LoginDTO.Response;

        Assert.That(signinResponse, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task Signin_ReturnsUnauthorized_WhenAdminNotFound()
    {
        var signinRequest = new LoginDTO.Request("admin@email.com", "Admin1@Test-123");

        var result = await _authenticationController.Signin(signinRequest);
        var unauthorizedResult = result as UnauthorizedObjectResult;

        Assert.That(unauthorizedResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(unauthorizedResult.StatusCode, Is.EqualTo(401));
            Assert.That(unauthorizedResult.Value, Is.EqualTo("Administrator not found."));
        });
    }

    [Test]
    public async Task Signin_ReturnsBadRequest_WhenSigninRequestIsNull()
    {
        var result = await _authenticationController.Signin(null);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Email and password are mandatory."));
        });
    }

    [Test]
    public async Task Signin_ReturnsBadRequest_WhenEmailIsNull()
    {
        var signinRequest = new LoginDTO.Request("", "Invalid@AdminPassword-123");

        var result = await _authenticationController.Signin(signinRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Email is required."));
        });
    }

    [Test]
    public async Task Signin_ReturnsBadRequest_WhenPasswordlIsNull()
    {
        var signinRequest = new LoginDTO.Request("invalidadmin@email.com", "");

        var result = await _authenticationController.Signin(signinRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Password is required."));
        });
    }

    [Test]
    public async Task Signup_ReturnsCreatedResult_WhenFieldsAreValid()
    {
        var admin = AdministratorsTests.CreateAdmin();
        var signupRequest = new SignupDTO.Request(admin.Name, admin.Email, admin.Password);

        _administratorRepositoryMock.Setup(repo => repo.CreateAdmin(It.IsAny<Administrators>())).ReturnsAsync(admin);

        var result = await _authenticationController.Signup(signupRequest);
        var createdResult = result as CreatedResult;
        var signupResponse = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(createdResult, Is.Not.Null);
        Assert.That(signupResponse, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(signupResponse.Message, Is.EqualTo("Administrator created successfully."));
            Assert.That(signupResponse.Id, Is.EqualTo(admin.Id));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenSignupRequestIsNull()
    {
        var result = await _authenticationController.Signup(null);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("All fields are required."));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenNameIsNull()
    {
        var admin = AdministratorsTests.CreateAdmin();
        var signupRequest = new SignupDTO.Request("", admin.Email, admin.Password);

        var result = await _authenticationController.Signup(signupRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid input."));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenEmailIsNull()
    {
        var admin = AdministratorsTests.CreateAdmin();
        var signupRequest = new SignupDTO.Request(admin.Name, "", admin.Password);

        var result = await _authenticationController.Signup(signupRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid input."));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenPasswordIsNull()
    {
        var admin = AdministratorsTests.CreateAdmin();
        var signupRequest = new SignupDTO.Request(admin.Name, admin.Email, "");

        var result = await _authenticationController.Signup(signupRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid input."));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        var existingAdmin = AdministratorsTests.CreateAdmin();
        _context.Administrators.Add(existingAdmin);
        await _context.SaveChangesAsync();

        var signupRequest = new SignupDTO.Request("New Admin", existingAdmin.Email, "New@Admin-123");

        var result = await _authenticationController.Signup(signupRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Email already in use."));
        });
    }

    #region Private Methods

    private static string GenerateRandomKey()
    {
        byte[] keyBytes = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }
        string base64Key = Convert.ToBase64String(keyBytes);

        base64Key = base64Key.Replace('_', '/').Replace('-', '+');

        return base64Key;
    }

    #endregion
}
