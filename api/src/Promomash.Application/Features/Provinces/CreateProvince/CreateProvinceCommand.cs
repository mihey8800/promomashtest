using Mapster;
using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;
using Promomash.Domain.Entities;
using Promomash.Domain.Events;

namespace Promomash.Application.Features.Provinces.CreateProvince;

public class CreateProvinceCommand : IRequest<Result<CreateProvinceCommandResponse>>
{
    public Guid CountryId { get; set; }
    public required string Name { get; set; }
}

public class
    CreateProvinceCommandHandler : IRequestHandler<CreateProvinceCommand, Result<CreateProvinceCommandResponse>>
{
    private readonly IProvinceRepository _provinceRepository;
    private readonly ICountryRepository _countryRepository;

    public CreateProvinceCommandHandler(IProvinceRepository provinceRepository, ICountryRepository countryRepository)
    {
        _provinceRepository = provinceRepository;
        _countryRepository = countryRepository;
    }

    public async Task<Result<CreateProvinceCommandResponse>> Handle(CreateProvinceCommand request,
        CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetById(request.CountryId);
        if (country == null)
        {
            return Result.Fail(new NotFoundError($"Country with id {request.CountryId} cannot be found"));
        }

        var province = new Province { Id = Guid.NewGuid(), Name = request.Name, Country = country };
        province.Country = country;
        
        var addResult = await _provinceRepository.Add(province);
        if (addResult.IsSuccess)
        {
            province.AddDomainEvent(EntityCreatedEvent.WithEntity(province));
            var response = addResult.Value.Adapt<CreateProvinceCommandResponse>();
            return Result.Ok(response);
        }

        return Result.Fail(addResult.Errors);
    }
}