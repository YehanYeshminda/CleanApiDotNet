using Clean.Domain.Errors.UserProfileErrors;
using Clean.Domain.Validators.UserProfileValidators;

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
        var validator = new BasicInfoValidator();
        
        var objToValidate = new BasicInfo
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            Phone = phone,
            DateOfBirth = dateOfBirth,
            CurrentCity = currentCity
        };

        var validationResult = validator.Validate(objToValidate);

        if (validationResult.IsValid) return objToValidate;
        
        var exception = new UserProfileNotValidException();
        
        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }
        
        throw exception;
    }
}