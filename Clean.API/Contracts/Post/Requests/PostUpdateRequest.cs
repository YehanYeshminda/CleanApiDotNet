namespace Clean.API.Contracts.Post.Requests;

public class PostUpdateRequest
{
    public Guid PostId { get; set; }
    public string TextContext { get; set; }
}