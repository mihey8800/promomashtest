using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Promomash.Application.Contracts.Persistence;
using Promomash.Domain.Entities;

namespace Promomash.Infrastructure.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationMiddleware(RequestDelegate next,
        IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        _next = next;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachUserToContext(context, token);

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            using var scope = _serviceProvider.CreateScope();
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            if (context.Items["User"] == null)
            {
                var user = await scope.ServiceProvider.GetRequiredService<IUserRepository>().GetById(
                    userId,
                    x => new { x.Country, x.Province });
                context.Items["User"] = user;
            }
            else
            {
                var user = (User?)context.Items["User"];
                if (user?.Id != Guid.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value))
                {
                    user = await scope.ServiceProvider.GetRequiredService<IUserRepository>().GetById(
                        userId,
                        x => new { x.Country, x.Province });
                    context.Items["User"] = user;
                }
            }
        }
        catch (Exception ex)
        {
            //Do nothing if JWT validation fails
            // user is not attached to context so the request won't have access to secure routes
        }
    }
}