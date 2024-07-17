using Promomash.Domain.Common;

namespace Promomash.Domain.Entities;

/// <summary>
/// Province
/// </summary>
public class Province : BaseEntity
{
    /// <summary>
    /// Province name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Country
    /// </summary>
    public virtual Country Country { get; set; } = null!;
}