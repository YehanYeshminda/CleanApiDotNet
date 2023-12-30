using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.Posts.Commands;
using Clean.DAL;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Posts.CommandHandlers;

public class PostDeleteCommandHandler : IRequestHandler<PostDeleteCommand, OperationResult<Post>>
{
    private readonly DataContext _context;

    public PostDeleteCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<Post>> Handle(PostDeleteCommand request, CancellationToken cancellationToken)
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
            
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);
            
            result.Payload = post;
            return result;
        }
        catch (Exception ex)
        {
            var error = new Error { Code = ErrorCodes.ServerError, Message = $"{ex.Message}"};
            result.Errors.Add(error);
            result.IsError = true;
            return result;
        }
    }
}