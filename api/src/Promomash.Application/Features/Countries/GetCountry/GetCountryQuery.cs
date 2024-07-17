using Mapster;
using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;

namespace Promomash.Application.Features.Countries.GetCountry;

public class GetCountryQuery : IRequest<Result<GetCountryQueryResponse>>
{
    public Guid Id { get; set; }
}

public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, Result<GetCountryQueryResponse>>
{
    private readonly ICountryRepository _countryRepository;

    public GetCountryQueryHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<Result<GetCountryQueryResponse>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetById(request.Id, x=>x.Provinces);
        
        if (country == null) {
            return Result.Fail(new NotFoundError($"Country with id {request.Id} cannot be found"));
        }
        
        var response = country.Adapt<GetCountryQueryResponse>();
        
        return Result.Ok(response);
    }
}