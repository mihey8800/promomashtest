using Promomash.Domain.Entities;

namespace Promomash.Application.Contracts.Persistence;

public interface IUserRepository : IAsyncRepository<User>
{
    public Task<User?> Login(string login, string password);
    public Task<bool> Logout();

    public string GenerateToken(User user);
}