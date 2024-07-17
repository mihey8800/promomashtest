using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promomash.Application.Features.Countries.ChangeName;
using Promomash.Application.Features.Countries.CreateCountry;
using Promomash.Application.Features.Countries.DeleteCountry;
using Promomash.Application.Features.Countries.GetCountries;
using Promomash.Application.Features.Countries.GetCountry;
using Promomash.Host.Extensions;
using Promomash.Infrastructure.Authorization;
using Promomash.Infrastructure.Controllers;

namespace Promomash.Host.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class CountryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CountryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.List))]
    [HttpGet(Name = "GetCountries")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetCountriesQueryResponse>))]
    public async Task<ActionResult> GetCountries()
    {
        var result = await _mediator.Send(new GetCountriesQuery());
        return result.ToHttpResponse();
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Get))]
    [HttpGet("{id:Guid}", Name = "GetCountry")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCountriesQueryResponse))]
    public async Task<ActionResult> Get(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(new GetCountryQuery { Id = id });
        return result.ToHttpResponse();
    }

    [Authorize]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Create))]
    [HttpPost(Name = "CreateCountry")]
    public async Task<ActionResult> Create(CreateCountryCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }

    [Authorize]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Update))]
    [HttpPost("ChangeName", Name = "ChangeCountryName")]
    public async Task<ActionResult> ChangeName(ChangeNameCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [Authorize]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Delete))]
    [HttpDelete("{id:Guid}", Name = "DeleteCountry")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(new DeleteCountryCommand { Id = id });
        return result.ToHttpResponse();
    }
}