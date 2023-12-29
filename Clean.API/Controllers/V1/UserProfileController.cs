using AutoMapper;
using Clean.API.Contracts.UserProfile.Requests;
using Clean.API.Contracts.UserProfile.Responses;
using Clean.API.Filters;
using Clean.Application.UserProfiles.Commands;
using Clean.Application.UserProfiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V1;

[ApiVersion(("1.0"))]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class UserProfileController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserProfileController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUserProfiles()
    {
        var query = new GetAllUserProfiles();
        var response = await _mediator.Send(query);
        var userProfiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
        return Ok(userProfiles);
    }
    
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] UserProfileCreateUpdate profile)
    {
        var command = _mapper.Map<CreateUserCommand>(profile);
        var response = await _mediator.Send(command);
        var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : CreatedAtAction(nameof(GetUserProfileById), new { id = response.Payload.UserProfileId }, userProfile);
    }

    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileById { UserProfileId = Guid.Parse(id) };
        var response = await _mediator.Send(query);
            
        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);
            
        return Ok(userProfile);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateModel]
    [ValidateGuid("id")]
    public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UserProfileCreateUpdate updatedProfile)
    {
        var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(updatedProfile); 
        command.UserProfileId = Guid.Parse(id);
        var response = await _mediator.Send(command);
        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> DeleteUserProfile(string id)
    {
        
        var command = new DeleteUserProfileCommand { UserProfileId = Guid.Parse(id) };
        var response = await _mediator.Send(command);
        
        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }
}