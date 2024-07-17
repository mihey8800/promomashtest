using Promomash.Domain.Common;

namespace Promomash.Domain.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = default!;

    public virtual ICollection<Province> Provinces { get; init; }
}