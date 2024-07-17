namespace Promomash.Application.Features.Provinces.CreateProvince;

public class CreateProvinceCommandValidator : AbstractValidator<CreateProvinceCommand>
{
    public CreateProvinceCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(p => p.CountryId)
            .NotNull();
    }
}