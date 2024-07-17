namespace Promomash.EntityFramework.Initialization;

public interface IDatabaseInitializer
{
    Task Initialize(CancellationToken cancellationToken);
}