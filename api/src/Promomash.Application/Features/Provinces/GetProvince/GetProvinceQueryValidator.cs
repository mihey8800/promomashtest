using Promomash.Application.Contracts.Persistence;

namespace Promomash.Application.Features.Provinces.GetProvince;

public class GetProvinceQueryValidator : AbstractValidator<GetProvinceQuery>
{
	public GetProvinceQueryValidator(IProvinceRepository provinceRepository)
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEqual(Guid.Empty);
    }
}