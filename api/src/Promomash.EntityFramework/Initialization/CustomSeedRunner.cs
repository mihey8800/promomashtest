using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Promomash.EntityFramework.Initialization.Seed;

namespace Promomash.EntityFramework.Initialization;

public interface ICustomSeedRunner
{
    Task RunSeeders(bool isDevelopment);
}
public class CustomSeedRunner : ICustomSeedRunner
{
    private readonly ICustomSeeder[] _seeders;
    private readonly ILogger<CustomSeedRunner> _logger;

    public CustomSeedRunner(IServiceProvider serviceProvider, ILogger<CustomSeedRunner> logger)
    {
        _seeders = serviceProvider.GetServices<ICustomSeeder>().ToArray();
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isDevelopment">IsDevelopmentData runs only on Development</param>
    /// <returns></returns>
    public async Task RunSeeders(bool isDevelopment)
    {
        foreach (var seeder in _seeders.OrderBy(c => c.Order))
        {
            if (!seeder.IsDevelopmentData || (seeder.IsDevelopmentData && isDevelopment))
            {
                _logger.LogInformation("Running {type}", seeder.GetType().FullName);
                await seeder.Initialize();
            }
        }
    }
}