using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockFlow.Application.Constants;
using StockFlow.Application.Services;
using StockFlow.Domain.DTOs;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StockFlow.Server.Controllers;

[Route($"api/{AppSettings.ApiVersion}/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly AdministratorsService _adminService;
    private readonly JwtDTO _jwt;

    public AuthenticationController(ApplicationDbContext context, AdministratorsService adminService, JwtDTO jwt)
    {
        _context = context;
        _adminService = adminService;
        _jwt = jwt;
    }

    // POST api/v1/auth/signin
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> Signin([FromBody, Required] LoginDTO.Request loginRequest)
    {
        if (loginRequest == null) return BadRequest("Email and password are mandatory.");
        if (string.IsNullOrWhiteSpace(loginRequest.Email)) return BadRequest("Email is required.");
        if (string.IsNullOrWhiteSpace(loginRequest.Password)) return BadRequest("Password is required.");

        var user = await _context.Administrators.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user == null) return Unauthorized("Administrator not found.");
        if (!VerifyPassword(loginRequest.Password, user.Password)) return Unauthorized("Incorrect password.");

        var claims = new List<Claim>()
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Role, AppSettings.PolicyAdmin),
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new("Name", user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwt.Key));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwt.Issuer,
            Audience = _jwt.Audience,
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return Ok(new LoginDTO.Response(jwtToken));
    }

    // POST api/v1/auth/signup
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody, Required] SignupDTO.Request signupRequest)
    {
        if (signupRequest == null) return BadRequest("All fields are required.");
        if (string.IsNullOrWhiteSpace(signupRequest.Name) || string.IsNullOrWhiteSpace(signupRequest.Email) ||
            string.IsNullOrWhiteSpace(signupRequest.Password)) return BadRequest("Invalid input.");

        var existingAdmin = await _context.Administrators.FirstOrDefaultAsync(u => u.Email == signupRequest.Email);
        if (existingAdmin != null) return BadRequest("Email already in use.");

        var newAdmin = new Administrators()
        {
            Id = Guid.NewGuid(),
            Email = signupRequest.Email,
            Password = signupRequest.Password,
            Name = signupRequest.Name
        };

        var createdUser = await _adminService.CreateAdmin(newAdmin);
        var response = new ResponsesDTO.Creation("Administrator created successfully.", createdUser.Id);

        return Created(string.Empty, response);
    }

    #region Private Methods

    private static bool VerifyPassword(string providedPassword, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = hashBytes[..16];

        using var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        return hash.SequenceEqual(hashBytes[16..]);
    }

    #endregion
}
