using Clean.Domain.Errors.PostErrors;
using Clean.Domain.Validators.PostValidators;

namespace Clean.Domain.Aggregates.PostAggregate;

public class PostComment
{
    private PostComment()
    {
    }
    
    public Guid CommentId { get; private set; }
    public Guid PostId { get; private set; }
    public string Text { get; private set; }
    public Guid UserProfileId { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }
    
    // FACTORY METHOD: CREATE NEW COMMENT
    public static PostComment CreatePostComment(Guid postId, string text, Guid userProfileId)
    {
        var validator = new PostCommentValidator();
        
        var objToValidate = new PostComment
        {
            CommentId = Guid.NewGuid(),
            PostId = postId,
            Text = text,
            UserProfileId = userProfileId,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
        
        var validationResult = validator.Validate(objToValidate);
        
        if (validationResult.IsValid) return objToValidate;
        
        var exception = new PostCommentNotValidateException();
        
        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }
        
        throw exception;
    }
    
    // FACTORY METHOD: UPDATE EXISTING COMMENT TEXT
    public void UpdateCommentText(string text)
    {
        Text = text;
        LastModified = DateTime.UtcNow;
    }
}