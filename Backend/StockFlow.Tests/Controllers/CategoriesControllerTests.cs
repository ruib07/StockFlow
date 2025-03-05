using Moq;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Services;
using StockFlow.Domain.Entities;
using StockFlow.Server.Controllers;
using StockFlow.Domain.DTOs;
using StockFlow.Tests.Templates;

namespace StockFlow.Tests.Controllers;

[TestFixture]
public class CategoriesControllerTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private CategoriesService _categoriesService;
    private CategoriesController _categoriesController;

    [SetUp]
    public void Setup()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoriesService = new CategoriesService(_categoryRepositoryMock.Object);
        _categoriesController = new CategoriesController(_categoriesService);
    }

    [Test]
    public async Task GetCategories_ReturnsOkResult_WithCategories()
    {
        var categories = CategoriesTests.CreateCategories();

        _categoryRepositoryMock.Setup(repo => repo.GetCategories()).ReturnsAsync(categories);

        var result = await _categoriesController.GetCategories();
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as IEnumerable<Categories>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Count(), Is.EqualTo(2));
            Assert.That(response.First().Id, Is.EqualTo(categories[0].Id));
            Assert.That(response.Last().Id, Is.EqualTo(categories[1].Id));
        });
    }

    [Test]
    public async Task GetCategoryById_ReturnsOkResult_WithCategory()
    {
        var category = CategoriesTests.CreateCategories().First();

        _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(category.Id)).ReturnsAsync(category);

        var result = await _categoriesController.GetCategoryById(category.Id);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as Categories;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(category.Id));
            Assert.That(response.Name, Is.EqualTo(category.Name));
        });
    }

    [Test]
    public async Task CreateCategory_ReturnsCreatedResult_WhenCategoryIsCreated()
    {
        var newCategory = CategoriesTests.CreateCategories().First();

        _categoryRepositoryMock.Setup(repo => repo.CreateCategory(It.IsAny<Categories>())).ReturnsAsync(newCategory);

        var result = await _categoriesController.CreateCategory(newCategory);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as ResponsesDTO.Creation;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(response.Message, Is.EqualTo("Category created successfully."));
            Assert.That(response.Id, Is.EqualTo(newCategory.Id));
        });
    }

    [Test]
    public async Task UpdateCategory_ReturnsOkResult_WhenCategoryIsUpdated()
    {
        var category = CategoriesTests.CreateCategories().First();
        var updatedCategory = CategoriesTests.UpdateCategory(category.Id);

        _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(category.Id)).ReturnsAsync(category);
        _categoryRepositoryMock.Setup(repo => repo.UpdateCategory(It.IsAny<Categories>())).Returns(Task.CompletedTask);

        var result = await _categoriesController.UpdateCategory(category.Id, updatedCategory);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Category updated successfully."));
        });
    }

    [Test]
    public async Task DeleteCategory_ReturnsNoContentResult_WhenCategoryIsDeleted()
    {
        var category = CategoriesTests.CreateCategories().First();

        _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(category.Id)).ReturnsAsync(category);
        _categoryRepositoryMock.Setup(repo => repo.DeleteCategory(category.Id)).Returns(Task.CompletedTask);

        var result = await _categoriesController.DeleteCategory(category.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
