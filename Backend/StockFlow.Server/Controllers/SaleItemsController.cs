using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/saleitems")]
public class SaleItemsController : ControllerBase
{
    private readonly SaleItemsService _saleItemsService;

    public SaleItemsController(SaleItemsService saleItemsService)
    {
        _saleItemsService = saleItemsService;
    }

    // GET api/v1/saleitems/{saleItemId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{saleItemId}")]
    public async Task<ActionResult<SaleItems>> GetSaleItemById([FromRoute] Guid saleItemId)
    {
        var saleItem = await _saleItemsService.GetSaleItemById(saleItemId);

        return Ok(saleItem);
    }

    // GET api/v1/saleitems/bysale/{saleId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("bysale/{saleId}")]
    public async Task<ActionResult<IEnumerable<SaleItems>>> GetSaleItemsBySaleId([FromRoute] Guid saleId)
    {
        var saleItemsBySale = await _saleItemsService.GetSaleItemsBySaleId(saleId);

        return Ok(saleItemsBySale);
    }

    // POST api/v1/saleitems
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPost]
    public async Task<ActionResult<SaleItems>> CreateSaleItem([FromBody] SaleItems saleItem)
    {
        var createdSaleItem = await _saleItemsService.CreateSaleItem(saleItem);

        var response = new ResponsesDTO.Creation("Sale item created successfully.", createdSaleItem.Id);

        return CreatedAtAction(nameof(GetSaleItemById), new { saleItemId = createdSaleItem.Id }, response);
    }

    // PUT api/v1/saleitems/{saleItemId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{saleItemId}")]
    public async Task<ActionResult<SaleItems>> UpdateSaleItem(Guid saleItemId, [FromBody] SaleItems updatedSaleItem)
    {
        await _saleItemsService.UpdateSaleItem(saleItemId, updatedSaleItem);

        return Ok("Sale item updated successfully.");
    }

    // DELETE api/v1/saleitems/{saleItemId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{saleItemId}")]
    public async Task<IActionResult> DeleteSaleItem(Guid saleItemId)
    {
        await _saleItemsService.DeleteSaleItem(saleItemId);

        return NoContent();
    }
}
