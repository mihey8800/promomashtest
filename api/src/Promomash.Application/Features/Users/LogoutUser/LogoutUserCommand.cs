using Promomash.Application.Contracts.Persistence;

namespace Promomash.Application.Features.Users.LogoutUser;

public class LogoutUserCommand : IRequest<Result<bool>>
{
}

public class
    LogoutUserCommandHandler(IUserRepository userService) : IRequestHandler<LogoutUserCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(LogoutUserCommand request,
        CancellationToken cancellationToken)
    {
        return await userService.Logout();
    }
}