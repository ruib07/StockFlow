using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoriesService;

    public CategoriesController(CategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    // GET api/v1/categories
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
    {
        var categories = await _categoriesService.GetCategories();

        return Ok(categories);
    }

    // GET api/v1/categories/{categoryId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{categoryId}")]
    public async Task<ActionResult<Categories>> GetCategoryById([FromRoute] Guid categoryId)
    {
        var category = await _categoriesService.GetCategoryById(categoryId);

        return Ok(category);
    }

    // POST api/v1/categories
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPost]
    public async Task<ActionResult<Categories>> CreateCategory([FromBody] Categories category)
    {
        var createdCategory = await _categoriesService.CreateCategory(category);

        var response = new ResponsesDTO.Creation("Category created successfully.", createdCategory.Id);

        return CreatedAtAction(nameof(GetCategoryById), new { categoryId = createdCategory.Id }, response);
    }

    // PUT api/v1/categories/{categoryId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{categoryId}")]
    public async Task<ActionResult<Categories>> UpdateCategory(Guid categoryId, [FromBody] Categories updateCategory)
    {
        await _categoriesService.UpdateCategory(categoryId, updateCategory);

        return Ok("Category updated successfully.");
    }

    // DELETE api/v1/categories/{categoryId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
    {
        await _categoriesService.DeleteCategory(categoryId);

        return NoContent();
    }
}
