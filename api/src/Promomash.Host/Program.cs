using System.Text.Json.Serialization;
using NSwag.AspNetCore;
using Promomash.Application;
using Promomash.EntityFramework;
using Promomash.Host.Extensions;
using Promomash.Host.Infrastructure;
using Promomash.Infrastructure;
using Promomash.Infrastructure.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig)
    => loggerConfig.ReadFrom.Configuration(context.Configuration));
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.ConfigureKestrel(builder.Configuration, Log.Logger);

builder.AddInfrastructure();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("Content-Disposition")));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuthorizationMiddleware>();
app.UseSerilogRequestLogging();

await app.MigrateDatabase();
await app.InitializeDatabase(app.Environment.IsDevelopment() || app.Environment.IsEnvironment("It"));

app.UseOpenApi();
app.UseSwaggerUi(options =>
{
    options.OAuth2Client = new OAuth2ClientSettings
    {
        ClientId = nameof(Promomash),
        AppName = nameof(Promomash),
    };
});

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace Promomash.Host
{
    public class Program
    {
    }
}