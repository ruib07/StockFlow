using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;

    public ProductsController(ProductsService productsService)
    {
        _productsService = productsService;
    }

    // GET api/v1/products
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
    {
        var products = await _productsService.GetProducts();

        return Ok(products);
    }

    // GET api/v1/products/{productId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{productId}")]
    public async Task<ActionResult<Products>> GetProductById([FromRoute] Guid productId)
    {
        var product = await _productsService.GetProductById(productId);

        return Ok(product);
    }

    // GET api/v1/products/bycategory/{categoryId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("bycategory/{categoryId}")]
    public async Task<ActionResult<IEnumerable<Products>>> GetProductsByCategoryId([FromRoute] Guid categoryId)
    {
        var productsByCategory = await _productsService.GetProductsByCategoryId(categoryId);

        return Ok(productsByCategory);
    }

    // POST api/v1/products
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPost]
    public async Task<ActionResult<Products>> CreateProduct([FromBody] Products product)
    {
        var createdProduct = await _productsService.CreateProduct(product);

        var response = new ResponsesDTO.Creation("Product created successfully.", createdProduct.Id);

        return CreatedAtAction(nameof(GetProductById), new { productId = createdProduct.Id }, response);
    }

    // PUT api/v1/products/{productId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{productId}")]
    public async Task<ActionResult<Products>> UpdateProduct(Guid productId, [FromBody] Products updateProduct)
    {
        await _productsService.UpdateProduct(productId, updateProduct);

        return Ok("Product updated successfully.");
    }

    // DELETE api/v1/products/{productId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        await _productsService.DeleteProduct(productId);

        return NoContent();
    }
}
