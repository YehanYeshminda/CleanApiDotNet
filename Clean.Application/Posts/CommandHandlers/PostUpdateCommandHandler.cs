using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.Posts.Commands;
using Clean.DAL;
using Clean.Domain.Aggregates.PostAggregate;
using Clean.Domain.Errors.PostErrors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Posts.CommandHandlers;

public class PostUpdateCommandHandler : IRequestHandler<PostUpdateCommand, OperationResult<Post>>
{
    private readonly DataContext _context;

    public PostUpdateCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<Post>> Handle(PostUpdateCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        
        try
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId);
            
            if (post is null)
            {
                var error = new Error { Code = ErrorCodes.NotFound, Message = $"Post with id {request.PostId} not found."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }
            
            post.UpdatePostText(request.TextContext);
            await _context.SaveChangesAsync(cancellationToken);
            result.Payload = post;
            return result;
        }
        catch (PostNotValidException ex)
        {
            result.IsError = true;
            foreach (var errorMessage in ex.ValidationErrors)
            {
                var error = new Error { Code = ErrorCodes.ValidationError, Message = $"{errorMessage}"};
                result.Errors.Add(error);
            }
            return result;
        }
        catch (Exception e)
        {
            var error = new Error { Code = ErrorCodes.ServerError, Message = $"{e.Message}"};
            result.Errors.Add(error);
            result.IsError = true;
            return result;
        }
    }
}