using Promomash.Domain.Entities;

namespace Promomash.Application.Contracts.Persistence;

public interface ICountryRepository : IAsyncRepository<Country>
{
}