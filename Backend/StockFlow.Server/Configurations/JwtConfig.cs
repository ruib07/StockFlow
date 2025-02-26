using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StockFlow.Domain.DTOs;
using System.Text;

namespace StockFlow.Server.Configurations;

public static class JwtConfig
{
    public static void AddCustomApiSecurity(this IServiceCollection services, ConfigurationManager configuration)
    {
        JwtDTO securitySettings = configuration.GetSection("Jwt").Get<JwtDTO>();
        services.AddSingleton(securitySettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = securitySettings.Issuer,
                ValidAudience = securitySettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(securitySettings.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });
    }
}
