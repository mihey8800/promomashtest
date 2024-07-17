using Mapster;
using Promomash.Application.Contracts.Persistence;

namespace Promomash.Application.Features.Countries.GetCountries;

public class GetCountriesQuery : IRequest<Result<IEnumerable<GetCountriesQueryResponse>>>
{
}

public class
    GetCountriesListQueryHandler : IRequestHandler<GetCountriesQuery, Result<IEnumerable<GetCountriesQueryResponse>>>
{
    private readonly ICountryRepository _countryRepository;

    public GetCountriesListQueryHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<Result<IEnumerable<GetCountriesQueryResponse>>> Handle(GetCountriesQuery request,
        CancellationToken cancellationToken)
    {
        var countries = (await _countryRepository.ListAllWithoutTracking(x => x.Provinces)).OrderBy(x => x.Name);
        var response = countries.Adapt<IEnumerable<GetCountriesQueryResponse>>();

        return Result.Ok(response);
    }
}