using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Promomash.Application.Contracts.Authorization;
using Promomash.Domain.Entities;
using Promomash.EntityFramework.Context;
using Promomash.EntityFramework.Initialization;
using Promomash.Infrastructure.Services;
using IdentityUser = Promomash.Infrastructure.Authorization.IdentityUser;

namespace Promomash.Host.Infrastructure;

public static class Bootstrap
{
    public static async Task MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        await dbInitializer.Initialize(CancellationToken.None);
    }

    public static async Task InitializeDatabase(this WebApplication app, bool isDevelopment)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IApplicationDbSeeder>();
        await seeder.SeedDatabase(isDevelopment);
    }

    public static IServiceCollection AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = $"/Auth/Login";
            options.LogoutPath = $"/Auth/Logout";
            options.AccessDeniedPath = $"/Account/AccessDenied";
            options.Events = new CookieAuthenticationEvents()
            {
                OnRedirectToLogin = (ctx) =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        ctx.Response.StatusCode = 200;
                    return Task.CompletedTask;
                },
                OnRedirectToAccessDenied = (ctx) =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        ctx.Response.StatusCode = 200;
                    return Task.CompletedTask;
                }
            };
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 2;
            options.Password.RequiredUniqueChars = 2;

            options.Lockout.AllowedForNewUsers = false;

            options.User.RequireUniqueEmail = true;
        });

        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        builder.Services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMemoryCache();

        builder.Services.AddFluentValidationAutoValidation(conf => { conf.DisableDataAnnotationsValidation = true; })
            .AddValidatorsFromAssemblyContaining<Locator>()
            .AddValidatorsFromAssemblyContaining<Application.Locator>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        ConfigureMapper();

        return builder.Services;
    }

    private static void ConfigureMapper()
    {
        TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
        Mapster.TypeAdapterConfig.GlobalSettings.Default.Config.CreateMapExpression(
            new Mapster.Models.TypeTuple(typeof(User), typeof(IdentityUser)),
            Mapster.MapType.Projection);
    }
}