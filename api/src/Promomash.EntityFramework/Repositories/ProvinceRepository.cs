using Promomash.Application.Contracts.Persistence;
using Promomash.EntityFramework.Context;

namespace Promomash.EntityFramework.Repositories;

public class ProvinceRepository : BaseRepository<Province>, IProvinceRepository
{
    public ProvinceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}