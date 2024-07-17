using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promomash.Application.Features.Provinces.ChangeName;
using Promomash.Application.Features.Provinces.CreateProvince;
using Promomash.Application.Features.Provinces.DeleteProvince;
using Promomash.Application.Features.Provinces.GetProvinces;
using Promomash.Host.Extensions;
using Promomash.Infrastructure.Authorization;
using Promomash.Infrastructure.Controllers;

namespace Promomash.Host.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class ProvinceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProvinceController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.List))]
    [HttpGet(Name = "GetProvinces")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetProvincesQueryResponse>))]
    public async Task<ActionResult> GetProvinces()
    {
        var result = await _mediator.Send(new GetProvincesQuery());
        return result.ToHttpResponse();
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Get))]
    [HttpGet("{id:Guid}", Name = "GetProvince")]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(Application.Features.Provinces.GetProvince.GetProvinceQueryResponse))]
    public async Task<ActionResult> Get(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(new Application.Features.Provinces.GetProvince.GetProvinceQuery { Id = id });
        return result.ToHttpResponse();
    }

    [Authorize]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Create))]
    [HttpPost(Name = "CreateProvince")]
    public async Task<ActionResult> Create(CreateProvinceCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }

    [Authorize]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Delete))]
    [HttpDelete("{id:Guid}", Name = "DeleteProvince")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(new DeleteProvinceCommand { Id = id });
        return result.ToHttpResponse();
    }

    [Authorize]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Update))]
    [HttpPost("ChangeName", Name = "ChangeProvinceName")]
    public async Task<ActionResult> ChangeName(ChangeNameCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }
}