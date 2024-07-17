using Promomash.Domain.Common;

namespace Promomash.Domain.Entities;

public class User : BaseEntity
{
    /// <summary>
    /// Login
    /// </summary>
    public required string Login { get; init; }

    /// <summary>
    /// Password
    /// </summary>
    public required string PasswordHash { get; init; }

    /// <summary>
    /// Country
    /// </summary>
    public virtual required Country Country { get; init; }

    /// <summary>
    /// Province
    /// </summary>
    public virtual required Province Province { get; init; }
}