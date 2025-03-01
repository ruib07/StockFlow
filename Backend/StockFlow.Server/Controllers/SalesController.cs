using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/sales")]
public class SalesController : ControllerBase
{
    private readonly SalesService _salesService;

    public SalesController(SalesService salesService)
    {
        _salesService = salesService;
    }

    // GET api/v1/sales
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sales>>> GetSales()
    {
        var sales = await _salesService.GetSales();

        return Ok(sales);
    }

    // GET api/v1/sales/{saleId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{saleId}")]
    public async Task<ActionResult<Sales>> GetSaleById([FromRoute] Guid saleId)
    {
        var sale = await _salesService.GetSaleById(saleId);

        return Ok(sale);
    }

    // POST api/v1/sales
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPost]
    public async Task<ActionResult<Sales>> CreateSale([FromBody] Sales sale)
    {
        var createdSale = await _salesService.CreateSale(sale);

        var response = new ResponsesDTO.Creation("Sale created successfully.", createdSale.Id);

        return CreatedAtAction(nameof(GetSaleById), new { saleId = createdSale.Id }, response);
    }

    // PUT api/v1/sales/{saleId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{saleId}")]
    public async Task<ActionResult<Sales>> UpdateSale(Guid saleId, [FromBody] Sales updateSale)
    {
        await _salesService.UpdateSale(saleId, updateSale);

        return Ok("Sale updated successfully.");
    }

    // DELETE api/v1/sales/{saleId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{saleId}")]
    public async Task<IActionResult> DeleteSale(Guid saleId)
    {
        await _salesService.DeleteSale(saleId);

        return NoContent();
    }
}
