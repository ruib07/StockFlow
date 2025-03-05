using Moq;
using StockFlow.Application.Helpers;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using StockFlow.Tests.Templates;
using System.Net;

namespace StockFlow.Tests.Services;

[TestFixture]
public class CategoriesServiceTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private CategoriesService _categoriesService;

    [SetUp]
    public void Setup()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoriesService = new CategoriesService(_categoryRepositoryMock.Object);
    }

    [Test]
    public async Task GetCategories_ReturnsCategories()
    {
        var categories = CategoriesTests.CreateCategories();

        _categoryRepositoryMock.Setup(repo => repo.GetCategories()).ReturnsAsync(categories);

        var result = await _categoriesService.GetCategories();

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(categories[0].Id));
            Assert.That(result.First().Name, Is.EqualTo(categories[0].Name));
            Assert.That(result.Last().Id, Is.EqualTo(categories[1].Id));
            Assert.That(result.Last().Name, Is.EqualTo(categories[1].Name));
        });
    }

    [Test]
    public async Task GetCategoryById_ReturnsCategory()
    {
        var category = CategoriesTests.CreateCategories().First();

        _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(category.Id)).ReturnsAsync(category);

        var result = await _categoriesService.GetCategoryById(category.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(category.Id));
            Assert.That(result.Name, Is.EqualTo(category.Name));
        });
    }

    [Test]
    public void GetCategoryById_ReturnsNotFound_WhenCategoryNotFound()
    {
        _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<Guid>())).ReturnsAsync((Categories)null);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _categoriesService.GetCategoryById(Guid.NewGuid()));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Category not found!"));
        });
    }

    [Test]
    public async Task CreateCategory_CreatesSuccessfully()
    {
        var category = CategoriesTests.CreateCategories().First();

        _categoryRepositoryMock.Setup(repo => repo.CreateCategory(category)).ReturnsAsync(category);

        var result = await _categoriesService.CreateCategory(category);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(category.Id));
            Assert.That(result.Name, Is.EqualTo(category.Name));
        });
    }

    [Test]
    public async Task UpdateCategory_UpdatesSuccessfully()
    {
        var category = CategoriesTests.CreateCategories().First();
        var updateCategory = CategoriesTests.UpdateCategory(category.Id);

        _categoryRepositoryMock.Setup(repo => repo.CreateCategory(category)).ReturnsAsync(category);
        _categoryRepositoryMock.Setup(repo => repo.UpdateCategory(updateCategory)).Returns(Task.CompletedTask);
        _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(category.Id)).ReturnsAsync(category);

        await _categoriesService.UpdateCategory(category.Id, updateCategory);
        var result = await _categoriesService.GetCategoryById(category.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateCategory.Id));
            Assert.That(result.Name, Is.EqualTo(updateCategory.Name));
        });
    }

    [Test]
    public async Task DeleteCategory_DeletesSuccessfully()
    {
        var category = CategoriesTests.CreateCategories().First();

        _categoryRepositoryMock.Setup(repo => repo.CreateCategory(category)).ReturnsAsync(category);
        _categoryRepositoryMock.Setup(repo => repo.DeleteCategory(category.Id)).Returns(Task.CompletedTask);
        _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(category.Id)).ReturnsAsync((Categories)null);

        await _categoriesService.CreateCategory(category);
        await _categoriesService.DeleteCategory(category.Id);

        var exception = Assert.ThrowsAsync<CustomException>(async () => await _categoriesService.GetCategoryById(category.Id));

        Assert.Multiple(() =>
        {
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Category not found!"));
        });
    }
}
