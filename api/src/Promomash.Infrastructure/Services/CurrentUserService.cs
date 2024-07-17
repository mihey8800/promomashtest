using Microsoft.AspNetCore.Http;
using Promomash.Application.Contracts.Authorization;

namespace Promomash.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly List<string> _roles;
    private readonly List<Claim> _claims;
    private readonly string _login;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor?.HttpContext;
        _roles = context?.User?.Claims?.Where(x => x.Type == ClaimTypes.Role)?.Select(x => x.Value).ToList() ??
                 new List<string>();
        _login = context?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
    }

    public string Login => _login;
    public List<string> Roles => _roles;
    public List<Claim> Claims => _claims;
}