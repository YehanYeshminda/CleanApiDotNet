namespace Clean.API.Contracts.Post.Requests;

public class PostCommentCreateRequest
{
    public string Text { get; set; }
    public string UserProfileId { get; set; }
}