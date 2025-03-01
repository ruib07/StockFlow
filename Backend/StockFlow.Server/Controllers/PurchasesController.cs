using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/purchases")]
public class PurchasesController : ControllerBase
{
    private readonly PurchasesService _purchasesService;

    public PurchasesController(PurchasesService purchasesService)
    {
        _purchasesService = purchasesService;
    }

    // GET api/v1/purchases
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Purchases>>> GetPurchases()
    {
        var purchases = await _purchasesService.GetPurchases();

        return Ok(purchases);
    }

    // GET api/v1/purchases/{purchaseId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{purchaseId}")]
    public async Task<ActionResult<Purchases>> GetPurchaseById([FromRoute] Guid purchaseId)
    {
        var purchase = await _purchasesService.GetPurchaseById(purchaseId);

        return Ok(purchase);
    }

    // POST api/v1/purchases
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPost]
    public async Task<ActionResult<Purchases>> CreatePurchase([FromBody] Purchases purchase)
    {
        var createdPurchase = await _purchasesService.CreatePurchase(purchase);

        var response = new ResponsesDTO.Creation("Purchase created successfully.", createdPurchase.Id);

        return CreatedAtAction(nameof(GetPurchaseById), new { purchaseId = createdPurchase.Id }, response);
    }

    // PUT api/v1/purchases/{purchaseId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{purchaseId}")]
    public async Task<ActionResult<Purchases>> UpdatePurchase(Guid purchaseId, [FromBody] Purchases updatePurchase)
    {
        await _purchasesService.UpdatePurchase(purchaseId, updatePurchase);

        return Ok("Purchase updated successfully.");
    }

    // DELETE api/v1/purchases/{purchaseId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{purchaseId}")]
    public async Task<IActionResult> DeletePurchase(Guid purchaseId)
    {
        await _purchasesService.DeletePurchase(purchaseId);

        return NoContent();
    }
}
