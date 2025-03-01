using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/purchaseitems")]
public class PurchaseItemsController : ControllerBase
{
    private readonly PurchaseItemsService _purchaseItemsService;

    public PurchaseItemsController(PurchaseItemsService purchaseItemsService)
    {
        _purchaseItemsService = purchaseItemsService;
    }

    // GET api/v1/purchaseitems/{purchaseItemId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{purchaseItemId}")]
    public async Task<ActionResult<PurchaseItems>> GetPurchaseItemById([FromRoute] Guid purchaseItemId)
    {
        var purchaseItem = await _purchaseItemsService.GetPurchaseItemById(purchaseItemId);

        return Ok(purchaseItem);
    }

    // GET api/v1/purchaseitems/bypurchase/{purchaseId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("bypurchase/{purchaseId}")]
    public async Task<ActionResult<IEnumerable<PurchaseItems>>> GetPurchaseItemsByPurchaseId([FromRoute] Guid purchaseId)
    {
        var purchaseItemsByPurchase = await _purchaseItemsService.GetPurchaseItemsByPurchaseId(purchaseId);

        return Ok(purchaseItemsByPurchase);
    }

    // POST api/v1/purchaseitems
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPost]
    public async Task<ActionResult<PurchaseItems>> CreatePurchaseItem([FromBody] PurchaseItems purchaseItem)
    {
        var createdPurchaseItem = await _purchaseItemsService.CreatePurchaseItem(purchaseItem);

        var response = new ResponsesDTO.Creation("Purchase item created successfully.", createdPurchaseItem.Id);

        return CreatedAtAction(nameof(GetPurchaseItemById), new { purchaseItemId = createdPurchaseItem.Id }, response);
    }

    // PUT api/v1/purchaseitems/{purchaseItemId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{purchaseItemId}")]
    public async Task<ActionResult<PurchaseItems>> UpdatePurchaseItem(Guid purchaseItemId, [FromBody] PurchaseItems updatePurchaseItems)
    {
        await _purchaseItemsService.UpdatePurchaseItem(purchaseItemId, updatePurchaseItems);

        return Ok("Purchase item updated successfully.");
    }

    // DELETE api/v1/purchaseitems/{purchaseItemId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{purchaseItemId}")]
    public async Task<IActionResult> DeletePurchaseItem(Guid purchaseItemId)
    {
        await _purchaseItemsService.DeletePurchaseItem(purchaseItemId);

        return NoContent();
    }
}
