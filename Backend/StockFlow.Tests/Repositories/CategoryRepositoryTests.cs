using Microsoft.EntityFrameworkCore;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repositories;
using StockFlow.Tests.Templates;

namespace StockFlow.Tests.Repositories;

[TestFixture]
public class CategoryRepositoryTests
{
    private CategoryRepository _categoryRepository;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);
        _categoryRepository = new CategoryRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetCategories_ReturnsCategories()
    {
        var categories = CategoriesTests.CreateCategories();
        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();

        var result = await _categoryRepository.GetCategories();

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

        await _categoryRepository.CreateCategory(category);

        var result = await _categoryRepository.GetCategoryById(category.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(category.Id));
            Assert.That(result.Name, Is.EqualTo(category.Name));
        });
    }

    [Test]
    public async Task CreateCategory_CreatesSuccessfully()
    {
        var newCategory = CategoriesTests.CreateCategories().First();

        var result = await _categoryRepository.CreateCategory(newCategory);
        var addedCategory = await _categoryRepository.GetCategoryById(newCategory.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(newCategory.Id));
            Assert.That(result.Name, Is.EqualTo(newCategory.Name));
        });
    }

    [Test]
    public async Task UpdateCategory_UpdatesSuccessfully()
    {
        var existingCategory = CategoriesTests.CreateCategories().First();
        await _categoryRepository.CreateCategory(existingCategory);

        _context.Entry(existingCategory).State = EntityState.Detached;

        var updatedCategory = CategoriesTests.UpdateCategory(existingCategory.Id);

        await _categoryRepository.UpdateCategory(updatedCategory);
        var retrievedUpdatedCategory = await _categoryRepository.GetCategoryById(existingCategory.Id);

        Assert.That(retrievedUpdatedCategory, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedCategory.Id, Is.EqualTo(updatedCategory.Id));
            Assert.That(retrievedUpdatedCategory.Name, Is.EqualTo(updatedCategory.Name));
        });
    }

    [Test]
    public async Task DeleteCategory_DeletesSuccessfully()
    {
        var existingCategory = CategoriesTests.CreateCategories().First();

        await _categoryRepository.CreateCategory(existingCategory);
        await _categoryRepository.DeleteCategory(existingCategory.Id);
        var retrievedEmptyCategory = await _categoryRepository.GetCategoryById(existingCategory.Id);

        Assert.That(retrievedEmptyCategory, Is.Null);
    }
}
