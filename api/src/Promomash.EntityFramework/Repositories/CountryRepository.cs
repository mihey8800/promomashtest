using Promomash.Application.Contracts.Persistence;
using Promomash.EntityFramework.Context;

namespace Promomash.EntityFramework.Repositories;

public class CountryRepository : BaseRepository<Country>, ICountryRepository
{
    public CountryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }   
}