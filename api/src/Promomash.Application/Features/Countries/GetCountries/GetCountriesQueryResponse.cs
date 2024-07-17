using Promomash.Application.Features.Provinces.GetProvince;

namespace Promomash.Application.Features.Countries.GetCountries;

public class GetCountriesQueryResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IReadOnlyCollection<GetProvinceQueryResponse> Provinces { get; set; } = null!;
}