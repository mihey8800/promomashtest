using Promomash.Domain.Entities;

namespace Promomash.Infrastructure.Authorization;

public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser
{
    public Country Country { get; set; }
    public Province Province { get; set; }

    public static IdentityUser FromDomainUser(User user)
    {
        return new IdentityUser
        {
            Id = user.Id.ToString(),
            UserName = user.Login,
            Email = user.Login,
            PasswordHash = user.PasswordHash,
            Country = user.Country,
            Province = user.Province
        };
    }

    public User? ToDomainUser()
    {
        return new User
        {
            Id = Guid.Parse(Id),
            Login = UserName,
            PasswordHash = PasswordHash,
            Country = Country,
            Province = Province
        };
    }
}