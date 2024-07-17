using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;
using Promomash.Domain.Events;

namespace Promomash.Application.Features.Provinces.DeleteProvince;

public class DeleteProvinceCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteProvinceCommandHandler : IRequestHandler<DeleteProvinceCommand, Result<Unit>>
{
    private readonly IProvinceRepository _provinceRepository;

    public DeleteProvinceCommandHandler(IProvinceRepository provinceRepository)
    {
        _provinceRepository = provinceRepository;
    }

    public async Task<Result<Unit>> Handle(DeleteProvinceCommand request, CancellationToken cancellationToken)
    {
        var province = await _provinceRepository.GetById(request.Id);

        if (province == null)
        {
            return Result.Fail(new NotFoundError($"Province with id {request.Id} cannot be found"));
        }

        
        await _provinceRepository.Delete(province);
        province.AddDomainEvent(EntityDeletedEvent.WithEntity(province));
        return Result.Ok(Unit.Value);
    }
}