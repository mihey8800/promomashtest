namespace Promomash.Application.Features.Provinces.DeleteProvince;

public class DeleteProvinceCommandValidator : AbstractValidator<DeleteProvinceCommand>
{
	public DeleteProvinceCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEqual(Guid.Empty);
    }
}
