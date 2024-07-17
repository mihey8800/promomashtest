namespace Promomash.Application.Features.Provinces.ChangeName;

public class ChangeNameCommandValidator : AbstractValidator<ChangeNameCommand>
{
    public ChangeNameCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEqual(Guid.Empty);

        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(250);
    }
}
