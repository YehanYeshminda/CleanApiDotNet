using AutoMapper;
using Clean.API.Contracts.Common;
using Clean.API.Contracts.Post.Requests;
using Clean.API.Contracts.Post.Responses;
using Clean.API.Filters;
using Clean.Application.Enums;
using Clean.Application.Posts.Commands;
using Clean.Application.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V1;

[ApiVersion(("1.0"))]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class PostsController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public PostsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPost()
    {
        var query = new GetAllPostsQuery();
        var response = await _mediator.Send(query);

        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        var mappedResponse = _mapper.Map<IEnumerable<PostResponse>>(response.Payload);
        return Ok(mappedResponse);
    }
    
    [HttpGet]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetById(string id)
    {
        var query = new GetPostByIdQuery { PostId = Guid.Parse(id) };
        var response = await _mediator.Send(query);
        var mappedResponse = _mapper.Map<PostResponse>(response.Payload);
        
        if (response.IsError) return HandleErrorResponse(response.Errors);
        return Ok(mappedResponse);
    }
    
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateRequest post)
    {
        var command = new PostCreateCommand() { UserProfileId = post.UserProfileId, TextContext = post.TextContext };
        var response = await _mediator.Send(command);
        
        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        var mappedResponse = _mapper.Map<PostResponse>(response.Payload);
        return CreatedAtAction(nameof(GetById), new { id = response.Payload.UserProfileId }, mappedResponse);
    }
    
    [HttpPut]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePost(string id, [FromBody] PostUpdateRequest post)
    {
        var command = new PostUpdateCommand() { PostId = Guid.Parse(id), TextContext = post.TextContext };
        var response = await _mediator.Send(command);
        
        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        var mappedResponse = _mapper.Map<PostResponse>(response.Payload);
        return Ok(mappedResponse);
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var command = new PostDeleteCommand() { PostId = Guid.Parse(id) };
        var response = await _mediator.Send(command);

        if (response.IsError) return HandleErrorResponse(response.Errors);

        var mappedResponse = _mapper.Map<PostResponse>(response.Payload);
        return Ok(mappedResponse);
    }
    
    [HttpGet]
    [Route(ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetCommentByPostId(string postId)
    {
        var query = new GetCommentByPostIdQuery { PostId = Guid.Parse(postId) };
        var response = await _mediator.Send(query);

        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        var mappedResponse = _mapper.Map<IEnumerable<PostCommentResponse>>(response.Payload);
        return Ok(mappedResponse);
    }
    
    [HttpPost]
    [Route(ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> CreatePostComment(string postId, [FromBody] PostCommentCreateRequest comment)
    {
        var isValidGuid = Guid.TryParse(postId, out var guid);

        if (!isValidGuid)
        {
            var apiError = new ErrorResponse();

            apiError.StatusCode = (int)ErrorCodes.ServerError;
            apiError.StatusPhase = "Internal Server Error";
            apiError.TimeStamp = DateTime.Now;

            apiError.Errors.Add("Provided user profile Id is not a valid Guid");
            return BadRequest(apiError);
        }
        
        var command = new PostCommentCreateCommand() { PostId = Guid.Parse(postId), TextContext = comment.Text, UserProfileId = Guid.Parse(comment.UserProfileId) };
        var response = await _mediator.Send(command);
        
        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        var mappedResponse = _mapper.Map<PostCommentResponse>(response.Payload);
        return Ok(mappedResponse);
    }
}