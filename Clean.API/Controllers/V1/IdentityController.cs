using AutoMapper;
using Clean.API.Contracts.Identity;
using Clean.API.Filters;
using Clean.Application.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
public class IdentityController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IdentityController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpPost]
    [Route(ApiRoutes.Identity.Register)]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] UserRegistrationContract request)
    {
        var command = _mapper.Map<RegisterIdentityCommand>(request);
        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var authenticationResult = new AuthenticationResponse() { Token = result.Payload };

        return Ok(authenticationResult);
    }
    
    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] LoginContract request)
    {
        var command = _mapper.Map<LoginIdentityCommand>(request);
        var result = await _mediator.Send(command);
        
        if (result.IsError) return HandleErrorResponse(result.Errors);
        
        var authenticationResult = new AuthenticationResponse() { Token = result.Payload };
        return Ok(authenticationResult);
    }
}