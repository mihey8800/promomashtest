namespace Promomash.Application.Features.Provinces.GetProvince;

public class GetProvinceQueryResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public Guid CountryId { get; set; }
}