namespace Clean.API;

public class ApiRoutes
{
    public const string BaseRoute = "api/v{version:apiVersion}/[controller]";
    
    public class UserProfiles
    {
        public const string IdRoute = "{id}";
    }
    
    public class Posts
    {
        public const string IdRoute = "{id}";
        public const string PostComments = "{postId}/comments";
        public const string CommentById = "comment/{commentId}";
    }

    public static class Identity
    {
        public const string Login = "login";
        public const string Register = "register";
    }
}