namespace Promomash.Application.Features.Countries.CreateCountry;

public class CreateCountryCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
