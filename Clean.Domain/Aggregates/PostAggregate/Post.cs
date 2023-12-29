using Clean.Domain.Aggregates.UserProfileAggregate;
using Clean.Domain.Errors.PostErrors;
using Clean.Domain.Validators.PostValidators;

namespace Clean.Domain.Aggregates.PostAggregate;

public class Post
{
    private readonly List<PostComment> _comments = new List<PostComment>();
    private readonly List<PostInteraction> _interactions = new List<PostInteraction>();
    
    private Post()
    {
    }
    
    public Guid PostId { get; private set; }
    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public string TextContext { get; private set; }
    public DateTime CreatedTime { get; private set; }
    public DateTime LastModified { get; private set; }
    public IEnumerable<PostComment> Comments
    {
        get { return _comments; }
    }
    public IEnumerable<PostInteraction> Interactions
    {
        get { return _interactions; }
    }
    
    // FACTORY METHOD: CREATE NEW POST
    public static Post CreatePost(Guid userProfileId, string textContext)
    {
        var validator = new PostValidator();
        
        var objToValidate = new Post
        {
            UserProfileId = userProfileId,
            TextContext = textContext,
            CreatedTime = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
        
        var validationResult = validator.Validate(objToValidate);
        
        if (validationResult.IsValid) return objToValidate;
        
        var exception = new PostNotValidException();
        
        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }
        
        throw exception;
    }
    
    // FACTORY METHOD: UPDATE EXISTING POST TEXT
    public void UpdatePostText(string textContext)
    {
        if (string.IsNullOrWhiteSpace(textContext))
        {
            var error = new PostNotValidException("Cannot update post. " + "Post text is not valid.");
            error.ValidationErrors.Add("Post text is not valid.");
            throw error;
        }

        TextContext = textContext;
        LastModified = DateTime.UtcNow;
    }
    
    // FACTORY METHOD: ADD NEW COMMENT TO POST
    public void AddComment(PostComment comment)
    {
        _comments.Add(comment);
        LastModified = DateTime.UtcNow;
    }
    
    // FACTORY METHOD: REMOVE COMMENT FROM POST
    public void RemoveComment(PostComment comment)
    {
        _comments.Remove(comment);
        LastModified = DateTime.UtcNow;
    }
    
    // FACTORY METHOD: ADD NEW INTERACTION TO POST
    public void AddInteraction(PostInteraction interaction)
    {
        _interactions.Add(interaction);
        LastModified = DateTime.UtcNow;
    }
    
    // FACTORY METHOD: REMOVE INTERACTION FROM POST
    public void RemoveInteraction(PostInteraction interaction)
    {
        _interactions.Remove(interaction);
        LastModified = DateTime.UtcNow;
    }
}