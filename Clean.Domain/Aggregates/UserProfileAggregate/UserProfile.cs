namespace Clean.Domain.Aggregates.UserProfileAggregate;

public class UserProfile
{
    private UserProfile()
    {
    }
    
    public Guid UserProfileId { get; private set; }
    public string IdentityId { get; private set; }
    public BasicInfo BasicInfo { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime LastModified { get; private set; }
    
    // FACTORY METHOD: CREATE NEW USER
    public static UserProfile CreateUserProfile(string IdentityId, BasicInfo basicInfo)
    {
        // TODO: Add validation error handling
        return new UserProfile
        {
            IdentityId = IdentityId,
            BasicInfo = basicInfo,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }
    
    // FACTORY METHOD: UPDATE EXISTING USER BASIC INFO
    public void UpdateBasicInfo(BasicInfo basicInfo)
    {
        BasicInfo = basicInfo;
        LastModified = DateTime.UtcNow;
    }
}