using Microsoft.Extensions.Logging;
using Promomash.EntityFramework.Context;

namespace Promomash.EntityFramework.Initialization;

public class ApplicationDbInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ApplicationDbInitializer> _logger;

    public ApplicationDbInitializer(ApplicationDbContext dbContext, ILogger<ApplicationDbInitializer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Initialize(CancellationToken cancellationToken)
    {
        if (!_dbContext.Database.IsInMemory() && _dbContext.Database.GetMigrations().Any() && (await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            _logger.LogInformation("Applying Migrations for Clean Architecture Db");
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }
    }
}
