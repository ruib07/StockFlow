using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly SuppliersService _suppliersService;

    public SuppliersController(SuppliersService suppliersService)
    {
        _suppliersService = suppliersService;
    }

    // GET api/v1/suppliers
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Suppliers>>> GetSuppliers()
    {
        var suppliers = await _suppliersService.GetSuppliers();

        return Ok(suppliers);
    }

    // GET api/v1/suppliers/{supplierId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{supplierId}")]
    public async Task<ActionResult<Suppliers>> GetSupplierById([FromRoute] Guid supplierId)
    {
        var supplier = await _suppliersService.GetSupplierById(supplierId);

        return Ok(supplier);
    }

    // POST api/v1/suppliers
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPost]
    public async Task<ActionResult<Suppliers>> CreateSupplier([FromBody] Suppliers supplier)
    {
        var createdSupplier = await _suppliersService.CreateSupplier(supplier);

        var response = new ResponsesDTO.Creation("Supplier created successfully.", createdSupplier.Id);

        return CreatedAtAction(nameof(GetSupplierById), new { supplierId = createdSupplier.Id }, response);
    }

    // PUT api/v1/suppliers/{supplierId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{supplierId}")]
    public async Task<ActionResult<Suppliers>> UpdateSupplier(Guid supplierId, [FromBody] Suppliers updateSupplier)
    {
        await _suppliersService.UpdateSupplier(supplierId, updateSupplier);

        return Ok("Supplier updated successfully.");
    }

    // DELETE api/v1/suppliers/{supplierId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{supplierId}")]
    public async Task<IActionResult> DeleteSupplier(Guid supplierId)
    {
        await _suppliersService.DeleteSupplier(supplierId);

        return NoContent();
    }
}
