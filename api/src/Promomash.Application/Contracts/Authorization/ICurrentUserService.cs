using System.Security.Claims;

namespace Promomash.Application.Contracts.Authorization;

public interface ICurrentUserService
{
    public string Login { get; }
    public List<string> Roles { get; }
    public List<Claim> Claims { get; }
}