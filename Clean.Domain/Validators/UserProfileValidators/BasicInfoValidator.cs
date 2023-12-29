using Clean.Domain.Aggregates.UserProfileAggregate;
using FluentValidation;

namespace Clean.Domain.Validators.UserProfileValidators;

public class BasicInfoValidator : AbstractValidator<BasicInfo>
{
    public BasicInfoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("First name is required.")
            .NotEmpty().WithMessage("First name cannot be empty.")
            .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.")
            .MinimumLength(3).WithMessage("First name cannot be less than 3 characters.");
        
        RuleFor(x => x.LastName)
            .NotNull().WithMessage("Last name is required.")
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.")
            .MinimumLength(3).WithMessage("Last name cannot be less than 3 characters.");

        RuleFor(x => x.EmailAddress)
            .NotNull().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Enter a valid email address.");

        RuleFor(x => x.DateOfBirth)
            .NotNull().WithMessage("Date of birth is required.")
            .Must(dob => dob >= DateTime.UtcNow.AddYears(-125) && dob <= DateTime.UtcNow.AddYears(-18))
            .WithMessage("Date of birth must be between 18 and 125 years old.");
    }
}