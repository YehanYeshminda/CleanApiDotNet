using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.Posts.Commands;
using Clean.DAL;
using Clean.Domain.Aggregates.PostAggregate;
using Clean.Domain.Errors.PostErrors;
using Clean.Domain.Validators.PostValidators;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Posts.CommandHandlers;

public class PostCommentCreateCommandHandler : IRequestHandler<PostCommentCreateCommand, OperationResult<PostComment>>
{
    private readonly DataContext _context;

    public PostCommentCreateCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<PostComment>> Handle(PostCommentCreateCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();
        
        try
        {
            var post = await _context.Posts.FirstOrDefaultAsync(X => X.PostId == request.PostId);
            
            if (post is null)
            {
                var error = new Error { Code = ErrorCodes.NotFound, Message = $"Post with id {request.PostId} not found."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }
            
            var comment = PostComment.CreatePostComment(request.PostId, request.TextContext, request.UserProfileId);
            post.AddComment(comment);
            
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            
            result.Payload = comment;
            return result;
        }
        catch (PostCommentNotValidateException ex)
        {
            result.IsError = true;
            foreach (var errorMessage in ex.ValidationErrors)
            {
                var error = new Error { Code = ErrorCodes.ValidationError, Message = $"{errorMessage}"};
                result.Errors.Add(error);
            }
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