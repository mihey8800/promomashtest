using Mapster;
using Promomash.Application.Contracts.Persistence;
using Promomash.Domain.Entities;
using Promomash.Domain.Events;

namespace Promomash.Application.Features.Countries.CreateCountry;

public class CreateCountryCommand : IRequest<Result<CreateCountryCommandResponse>>
{
    public string Name { get; set; } = default!;
}

public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Result<CreateCountryCommandResponse>>
{
    private readonly ICountryRepository _countryRepository;

    public CreateCountryCommandHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<Result<CreateCountryCommandResponse>> Handle(CreateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var country = new Country { Id = Guid.NewGuid() };
        request.Adapt(country);
        var addResult = await _countryRepository.Add(country);
        if (addResult.IsSuccess)
        {
            country = addResult.Value;
            country.AddDomainEvent(EntityCreatedEvent.WithEntity(country));
            var response = country.Adapt<CreateCountryCommandResponse>();

            return Result.Ok(response);
        }

        return Result.Fail(addResult.Errors);
    }
}