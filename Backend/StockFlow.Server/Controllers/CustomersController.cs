using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/customers")]
public class CustomersController : ControllerBase
{
    private readonly CustomersService _customersService;

    public CustomersController(CustomersService customersService)
    {
        _customersService = customersService;
    }

    // GET api/v1/customers
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
    {
        var customers = await _customersService.GetCustomers();

        return Ok(customers);
    }

    // GET api/v1/customers/{customerId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{customerId}")]
    public async Task<ActionResult<Customers>> GetCustomerById([FromRoute] Guid customerId)
    {
        var customer = await _customersService.GetCustomerById(customerId);

        return Ok(customer);
    }

    // POST api/v1/customers
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPost]
    public async Task<ActionResult<Customers>> CreateCustomer([FromBody] Customers customer)
    {
        var createdCustomer = await _customersService.CreateCustomer(customer);

        var response = new ResponsesDTO.Creation("Customer created successfully.", createdCustomer.Id);
         
        return CreatedAtAction(nameof(GetCustomerById), new { customerId = createdCustomer.Id }, response);
    }

    // PUT api/v1/customers/{customerId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{customerId}")]
    public async Task<ActionResult<Customers>> UpdateCustomer(Guid customerId, [FromBody] Customers updateCustomer)
    {
        await _customersService.UpdateCustomer(customerId, updateCustomer);

        return Ok("Customer updated successfully.");
    }

    // DELETE api/v1/customers/{customerId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomer(Guid customerId)
    {
        await _customersService.DeleteCustomer(customerId);

        return NoContent();
    }
}
