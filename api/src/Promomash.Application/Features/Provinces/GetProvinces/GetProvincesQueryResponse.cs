namespace Promomash.Application.Features.Provinces.GetProvinces;

public class GetProvincesQueryResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public Guid CountryId { get; set; }
}