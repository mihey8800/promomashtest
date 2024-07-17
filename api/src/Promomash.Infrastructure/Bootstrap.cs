using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Promomash.Infrastructure;

public static class Bootstrap
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenApiDocument(configure =>
        {
            configure.Title = nameof(Promomash);
            configure.AddSecurity(nameof(OpenApiSecuritySchemeType.OAuth2),
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Description = nameof(OpenApiSecuritySchemeType.OAuth2),
                    Flow = OpenApiOAuth2Flow.Password,
                    AuthorizationUrl = "/api/v1/auth/login",
                    TokenUrl = "/api/v1/auth/login",
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = "/api/v1/auth/login"
                        }
                    },
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Scopes = new Dictionary<string, string>
                    {
                        { nameof(OpenApiSecuritySchemeType.OAuth2), nameof(Promomash) }
                    }
                });

            configure.OperationProcessors.Add(
                new AspNetCoreOperationSecurityScopeProcessor(nameof(OpenApiSecuritySchemeType.OAuth2)));
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        return services;
    }
}