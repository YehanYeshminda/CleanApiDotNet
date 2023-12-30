namespace Clean.Application.Enums;

public enum ErrorCodes
{
    NotFound = 404,
    ServerError = 500,
    BadRequest = 400,
    ValidationError = 422,
    ExistingIdentityUser = 550,
    IdentityUserCreationFailed = 551,
}