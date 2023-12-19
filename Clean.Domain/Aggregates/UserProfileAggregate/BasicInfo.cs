namespace Clean.Domain.Aggregates.UserProfileAggregate;

public class BasicInfo
{
    private BasicInfo()
    {
    }
    
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string EmailAddress { get; private set; }
    public string Phone { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string CurrentCity { get; private set; }
    
    // FACTORY METHOD: CREATE NEW BASIC INFO
    public static BasicInfo CreateBasicInfo(string firstName, string lastName, string emailAddress, string phone, DateTime dateOfBirth, string currentCity) 
    {
        // TODO: Add validation error handling
        return new BasicInfo()
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            Phone = phone,
            DateOfBirth = dateOfBirth,
            CurrentCity = currentCity
        };
    }
}