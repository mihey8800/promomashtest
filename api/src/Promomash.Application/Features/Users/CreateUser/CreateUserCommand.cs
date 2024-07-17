using Mapster;
using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;
using Promomash.Domain.Entities;
using Promomash.Domain.Events;

namespace Promomash.Application.Features.Users.CreateUser;

public class CreateUserCommand : IRequest<Result<CreateUserCommandResponse>>
{
    public Guid CountryId { get; set; }
    public Guid ProvinceId { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
}

public class
    CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICountryRepository _countryRepository;

    public CreateUserCommandHandler(IUserRepository userRepository, ICountryRepository countryRepository)
    {
        _userRepository = userRepository;
        _countryRepository = countryRepository;
    }

    public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetById(request.CountryId, x => x.Provinces);
        if (country == null)
        {
            return Result.Fail(new NotFoundError($"Country with id {request.CountryId} cannot be found"));
        }

        var province = country.Provinces.FirstOrDefault(x => x.Id == request.ProvinceId);
        if (province == null)
        {
            return Result.Fail(new NotFoundError($"Province with id {request.ProvinceId} cannot be found"));
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Country = country,
            Province = province,
            Login = request.Login,
            PasswordHash = request.Password
        };

        var addResult = await _userRepository.Add(user);
        if (!addResult.IsSuccess) return Result.Fail(addResult.Errors);
        user.AddDomainEvent(EntityCreatedEvent.WithEntity(province));
        var response = addResult.Value.Adapt<CreateUserCommandResponse>();
        return Result.Ok(response);
    }
}