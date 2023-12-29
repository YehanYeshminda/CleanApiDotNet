namespace Clean.API.Contracts.Post.Responses;

public class PostResponse
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public string TextContext { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime LastModified { get; set; }
}