using Promomash.Application.Features.Provinces.GetProvince;

namespace Promomash.Application.Features.Countries.GetCountry;

public class GetCountryQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public IReadOnlyCollection<GetProvinceQueryResponse> Provinces { get; set; } = null!;
}