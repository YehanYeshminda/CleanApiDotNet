namespace Clean.API.Contracts.Post.Requests;

public class PostCreateRequest
{
    public Guid UserProfileId { get; set; }
    public string TextContext { get; set; }
}