namespace Promomash.Application.Features.Countries.GetCountry;

public class GetCountryQueryValidator : AbstractValidator<GetCountryQuery>
{
    public GetCountryQueryValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEqual(Guid.Empty); 
    }
}