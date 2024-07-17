using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;

namespace Promomash.Application.Features.Countries.ChangeName;

public class ChangeNameCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class ChangeCountryNameCommandHandler : IRequestHandler<ChangeNameCommand, Result<Unit>>
{
    private readonly ICountryRepository _countryRepository;

    public ChangeCountryNameCommandHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<Result<Unit>> Handle(ChangeNameCommand request, CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetById(request.Id);

        if (country == null)
        {
            return Result.Fail(new NotFoundError($"Country with id {request.Id} cannot be found"));
        }

        if (await _countryRepository.Exists(x => x.Name.ToLower() == request.Name.ToLower()))
        {
            return Result.Fail(new NotFoundError($"Country with same name {request.Name} already exists"));
        }

        country.Name = request.Name;
        await _countryRepository.Update(country);

        return Result.Ok(Unit.Value);
    }
}