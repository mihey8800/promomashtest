namespace Promomash.EntityFramework.Initialization.Seed;

public interface ICustomSeeder
{
    Task Initialize();
    bool IsDevelopmentData { get; }
    int Order { get; }
}
