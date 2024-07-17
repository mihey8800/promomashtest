using Mapster;
using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;

namespace Promomash.Application.Features.Provinces.GetProvince;

public class GetProvinceQuery : IRequest<Result<GetProvinceQueryResponse>>
{
    public Guid Id { get; set; }
}

public class GetProvinceQueryHandler : IRequestHandler<GetProvinceQuery, Result<GetProvinceQueryResponse>>
{
    private readonly IProvinceRepository _provinceRepository;

    public GetProvinceQueryHandler(IProvinceRepository provinceRepository)
    {
        _provinceRepository = provinceRepository;
    }

    public async Task<Result<GetProvinceQueryResponse>> Handle(GetProvinceQuery request,
        CancellationToken cancellationToken)
    {
        var province = await _provinceRepository.GetById(request.Id, x => x.Country);

        if (province == null)
        {
            return Result.Fail(new NotFoundError($"Province with id {request.Id} cannot be found"));
        }

        var response = province.Adapt<GetProvinceQueryResponse>();

        return Result.Ok(response);
    }
}