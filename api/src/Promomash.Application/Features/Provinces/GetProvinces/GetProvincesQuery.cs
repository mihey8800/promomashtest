using Mapster;
using Promomash.Application.Contracts.Persistence;

namespace Promomash.Application.Features.Provinces.GetProvinces;

public class GetProvincesQuery : IRequest<Result<IEnumerable<GetProvincesQueryResponse>>>
{
}

public class
    GetProvincesListQueryHandler(IProvinceRepository provinceRepository)
    : IRequestHandler<GetProvincesQuery, Result<IEnumerable<GetProvincesQueryResponse>>>
{
    public async Task<Result<IEnumerable<GetProvincesQueryResponse>>> Handle(GetProvincesQuery request,
        CancellationToken cancellationToken)
    {
        var provinces = (await provinceRepository.ListAllWithoutTracking(x => x.Country)).OrderBy(x => x.Name);

        var response = provinces.Adapt<IEnumerable<GetProvincesQueryResponse>>();

        return Result.Ok(response);
    }
}