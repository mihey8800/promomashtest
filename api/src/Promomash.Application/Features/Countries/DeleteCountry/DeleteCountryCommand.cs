using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;
using Promomash.Domain.Events;

namespace Promomash.Application.Features.Countries.DeleteCountry;

public class DeleteCountryCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result<Unit>>
{
    private readonly ICountryRepository _countryRepository;

    public DeleteCountryCommandHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<Result<Unit>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetById(request.Id);
        
        if (country == null)
        {
            return Result.Fail(new NotFoundError($"Country with id {request.Id} cannot be found"));
        }

        
        await _countryRepository.Delete(country);
        country.AddDomainEvent(EntityDeletedEvent.WithEntity(country));
        return Result.Ok(Unit.Value);
    }
}