using AutoMapper;
using Clean.API.Contracts.UserProfile.Requests;
using Clean.API.Contracts.UserProfile.Responses;
using Clean.Application.UserProfiles.Commands;
using Clean.Application.UserProfiles.Queries;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V1;

[ApiVersion(("1.0"))]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class UserProfileController : Controller
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
        try
        {
            var query = new GetAllUserProfiles();
            var response = await _mediator.Send(query);
            var userProfiles = _mapper.Map<List<UserProfileResponse>>(response);
        
            return Ok(userProfiles);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserProfileCreateUpdate profile)
    {
        try
        {
            // Destination to map is the command and map from the profile
            var command = _mapper.Map<CreateUserCommand>(profile);
            var response = await _mediator.Send(command);
            var userProfile = _mapper.Map<UserProfileResponse>(response);

            return CreatedAtAction(nameof(GetUserProfileById), new { id = response.UserProfileId }, userProfile);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        try
        {
            var query = new GetUserProfileById { UserProfileId = Guid.Parse(id) };
            var response = await _mediator.Send(query);

            if (response is null) return NotFound($"User with the {id} is not found");
            
            var userProfile = _mapper.Map<UserProfileResponse>(response);
            
            return Ok(userProfile);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UserProfileCreateUpdate updatedProfile)
    {
        try
        {
            var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(updatedProfile); 
            command.UserProfileId = Guid.Parse(id);
            var response = await _mediator.Send(command);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> DeleteUserProfile(string id)
    {
        try
        {
            var command = new DeleteUserProfileCommand { UserProfileId = Guid.Parse(id) };
            var response = await _mediator.Send(command);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}