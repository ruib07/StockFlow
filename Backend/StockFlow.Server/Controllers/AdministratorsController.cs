using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Application.Services.Email.Interfaces;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/administrators")]
public class AdministratorsController : ControllerBase
{
    private readonly AdministratorsService _administratorsService;
    private readonly IEmailPasswordReset _emailService;

    public AdministratorsController(AdministratorsService administratorsService, IEmailPasswordReset emailService)
    {
        _administratorsService = administratorsService;
        _emailService = emailService;
    }

    // GET api/v1/administrators/{adminId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpGet("{adminId}")]
    public async Task<ActionResult<Administrators>> GetAdminById([FromRoute] Guid adminId)
    {
        var admin = await _administratorsService.GetAdminById(adminId);

        return Ok(admin);
    }

    // POST api/v1/administrators/recoverpassword
    [HttpPost("recoverpassword")]
    public async Task<IActionResult> RecoverPasswordSendEmail([FromBody] RecoverPasswordDTO.Request request)
    {
        var token = await _administratorsService.GeneratePasswordResetToken(request.Email);

        await _emailService.SendPasswordResetEmail(request.Email, token);

        return Ok();
    }

    // PUT api/v1/administrators/updatepassword
    [HttpPut("updatepassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] RecoverPasswordDTO.UpdatePassword updatePassword)
    {
        await _administratorsService.UpdatePassword(updatePassword.Token, updatePassword.NewPassword, updatePassword.ConfirmNewPassword);

        return Ok();
    }

    // PUT api/v1/administrators/{adminId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpPut("{adminId}")]
    public async Task<ActionResult<Administrators>> UpdateAdmin(Guid adminId, [FromBody] Administrators updateAdmin)
    {
        await _administratorsService.UpdateAdmin(adminId, updateAdmin);

        return Ok("Administrator updated successfully.");
    }

    // DELETE api/v1/administrators/{adminId}
    [Authorize(Policy = AppSettings.PolicyAdmin)]
    [HttpDelete("{adminId}")]
    public async Task<IActionResult> DeleteAdmin(Guid adminId)
    {
        await _administratorsService.DeleteAdmin(adminId);

        return NoContent();
    }
}
