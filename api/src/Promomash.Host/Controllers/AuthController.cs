using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promomash.Application.Features.Users.CreateUser;
using Promomash.Application.Features.Users.LoginUser;
using Promomash.Application.Features.Users.LogoutUser;
using Promomash.Host.Extensions;
using Promomash.Infrastructure.Controllers;

namespace Promomash.Host.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class AuthController : BaseApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Create))]
    [HttpPost("Register")]
    public async Task<ActionResult> Register(CreateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType(typeof(ProblemDetails))]
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromForm] string username, [FromForm] string password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(new LoginUserCommand()
        {
            Login = username,
            Password = password
        });
        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost("Logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType(typeof(ProblemDetails))]
    public async Task<ActionResult> Logout(LogoutUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _mediator.Send(command);
        return Ok();
    }
}