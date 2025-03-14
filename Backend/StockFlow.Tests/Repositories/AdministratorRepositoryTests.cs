﻿using Microsoft.EntityFrameworkCore;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;
using StockFlow.Tests.Templates;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class AdministratorRepositoryTests
{
    private AdministratorRepository _administratorRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _administratorRepository = new AdministratorRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetAdminById_ReturnsAdmin()
    {
        var admin = AdministratorsTests.CreateAdmin();

        await _administratorRepository.CreateAdmin(admin);

        var result = await _administratorRepository.GetAdminById(admin.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(admin.Id));
            Assert.That(result.Name, Is.EqualTo(admin.Name));
            Assert.That(result.Email, Is.EqualTo(admin.Email));
        });
    }

    [Test]
    public async Task GetAdminByEmail_ReturnsAdmin()
    {
        var admin = AdministratorsTests.CreateAdmin();

        await _administratorRepository.CreateAdmin(admin);

        var result = await _administratorRepository.GetAdminByEmail(admin.Email);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(admin.Id));
            Assert.That(result.Name, Is.EqualTo(admin.Name));
            Assert.That(result.Email, Is.EqualTo(admin.Email));
        });
    }

    [Test]
    public async Task CreateAdmin_CreatesSuccessfully()
    {
        var newAdmin = AdministratorsTests.CreateAdmin();

        var result = await _administratorRepository.CreateAdmin(newAdmin);
        var addedAdmin = await _administratorRepository.GetAdminById(newAdmin.Id);

        Assert.That(addedAdmin, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedAdmin.Id, Is.EqualTo(newAdmin.Id));
            Assert.That(addedAdmin.Name, Is.EqualTo(newAdmin.Name));
            Assert.That(addedAdmin.Email, Is.EqualTo(newAdmin.Email));
        });
    }

    [Test]
    public async Task GeneratePasswordResetToken_CreatesTokenSuccessfully()
    {
        var existingAdmin = AdministratorsTests.CreateAdmin();

        await _administratorRepository.CreateAdmin(existingAdmin);
        var token = await _administratorRepository.GeneratePasswordResetToken(existingAdmin.Id);

        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public async Task GetPasswordResetToken_ReturnsToken()
    {
        var existingAdmin = AdministratorsTests.CreateAdmin();

        await _administratorRepository.CreateAdmin(existingAdmin);

        var token = await _administratorRepository.GeneratePasswordResetToken(existingAdmin.Id);
        var savedToken = await _administratorRepository.GetPasswordResetToken(token);

        Assert.Multiple(() =>
        {
            Assert.That(savedToken.AdminId, Is.EqualTo(existingAdmin.Id));
            Assert.That(savedToken.Token, Is.EqualTo(token));
            Assert.That(savedToken.ExpiryDate, Is.GreaterThan(DateTime.UtcNow));
        });
    }

    [Test]
    public async Task RemovePasswordResetToken_RemovesToken()
    {
        var existingAdmin = AdministratorsTests.CreateAdmin();

        await _administratorRepository.CreateAdmin(existingAdmin);

        var token = await _administratorRepository.GeneratePasswordResetToken(existingAdmin.Id);
        var savedToken = await _administratorRepository.GetPasswordResetToken(token);
        await _administratorRepository.RemovePasswordResetToken(savedToken);
        var deletedToken = await _administratorRepository.GetPasswordResetToken(token);

        Assert.Multiple(() =>
        {
            Assert.That(savedToken, Is.Not.Null);
            Assert.That(deletedToken, Is.Null);
        });
    }

    [Test]
    public async Task UpdateAdmin_UpdatesSuccessfully()
    {
        var existingAdmin = AdministratorsTests.CreateAdmin();
        await _administratorRepository.CreateAdmin(existingAdmin);

        _context.Entry(existingAdmin).State = EntityState.Detached;

        var updatedAdmin = AdministratorsTests.UpdateAdmin(existingAdmin.Id, "adminupdated@email.com");

        await _administratorRepository.UpdateAdmin(updatedAdmin);
        var retrievedUpdatedAdmin = await _administratorRepository.GetAdminById(existingAdmin.Id);

        Assert.That(retrievedUpdatedAdmin, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedAdmin.Name, Is.EqualTo(updatedAdmin.Name));
            Assert.That(retrievedUpdatedAdmin.Email, Is.EqualTo(updatedAdmin.Email));
        });
    }

    [Test]
    public async Task DeleteAdmin_DeletesSuccessfully()
    {
        var existingAdmin = AdministratorsTests.CreateAdmin();

        await _administratorRepository.CreateAdmin(existingAdmin);
        await _administratorRepository.DeleteAdmin(existingAdmin.Id);
        var retrivedEmptyAdmin = await _administratorRepository.GetAdminById(existingAdmin.Id);

        Assert.That(retrivedEmptyAdmin, Is.Null);
    }
}
