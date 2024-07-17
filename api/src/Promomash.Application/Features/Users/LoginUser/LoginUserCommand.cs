using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Validation;

namespace Promomash.Application.Features.Users.LoginUser;

public class LoginUserCommand : IRequest<Result<LoginUserCommandResponse>>
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}

public class
    LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserCommandResponse>>
{
    private readonly IUserRepository _userService;

    public LoginUserCommandHandler(IUserRepository userService)
    {
        _userService = userService;
    }

    public async Task<Result<LoginUserCommandResponse>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var loginResult = await _userService.Login(request.Login, request.Password);
        if (loginResult != null)
        {
            try
            {
                return Result.Ok(new LoginUserCommandResponse()
                {
                    Token = _userService.GenerateToken(loginResult)
                });
            }
            catch (Exception ex)
            {
                return Result.Fail(new ForbiddenAccessError("Error generating jwt token"));
            }
        }

        return Result.Fail(new ForbiddenAccessError("Wrong email or password"));
    }
}