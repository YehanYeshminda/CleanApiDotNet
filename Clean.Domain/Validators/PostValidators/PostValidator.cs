using Clean.Domain.Aggregates.PostAggregate;
using FluentValidation;

namespace Clean.Domain.Validators.PostValidators;

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(x => x.TextContext)
            .NotNull().WithMessage("Post text content is required.")
            .NotEmpty().WithMessage("Post text content cannot be empty.")
            .MaximumLength(100).WithMessage("Post text content cannot be longer than 100 characters.")
            .MinimumLength(3).WithMessage("Post text content cannot be less than 3 characters.");

        RuleFor(x => x.UserProfileId)
            .NotNull().WithMessage("Post user profile id is required.")
            .NotEmpty().WithMessage("Post user profile id cannot be empty.")
            .NotEqual(Guid.Empty).WithMessage("Post user profile id cannot be empty.")
            .Must(BeValidGuid).WithMessage("Post user profile id is not valid.");
    }
    
    private bool BeValidGuid(Guid userProfileId)
    {
        return userProfileId != Guid.Empty;
    }
}