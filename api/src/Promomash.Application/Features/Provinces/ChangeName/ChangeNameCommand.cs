using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;

namespace Promomash.Application.Features.Provinces.ChangeName;

public class ChangeNameCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class ChangeCountryNameCommandHandler : IRequestHandler<ChangeNameCommand, Result<Unit>>
{
    private readonly IProvinceRepository _provinceRepository;

    public ChangeCountryNameCommandHandler(IProvinceRepository provinceRepository)
    {
        _provinceRepository = provinceRepository;
    }

    public async Task<Result<Unit>> Handle(ChangeNameCommand request, CancellationToken cancellationToken)
    {
        var province = await _provinceRepository.GetById(request.Id, x => x.Country);

        if (province == null)
        {
            return Result.Fail(new NotFoundError($"Province with id {request.Id} cannot be found"));
        }

        if (await _provinceRepository.Exists(x =>
                    x.Name.ToLower() == request.Name.ToLower() && x.Country.Id == province.Country.Id,
                i => i.Country))
        {
            return Result.Fail(
                new NotFoundError($"Province with same name {request.Name} already exists in {province.Country.Name}"));
        }

        province.Name = request.Name;
        await _provinceRepository.Update(province);

        return Result.Ok(Unit.Value);
    }
}