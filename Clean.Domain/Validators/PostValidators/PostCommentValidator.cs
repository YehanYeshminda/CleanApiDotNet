using Clean.Domain.Aggregates.PostAggregate;
using FluentValidation;

namespace Clean.Domain.Validators.PostValidators;

public class PostCommentValidator : AbstractValidator<PostComment>
{
    public PostCommentValidator()
    {
        RuleFor(x => x.Text)
            .NotNull().WithMessage("Comment text is required.")
            .NotEmpty().WithMessage("Comment text cannot be empty.")
            .MaximumLength(500).WithMessage("Comment text cannot be longer than 500 characters.")
            .MinimumLength(3).WithMessage("Comment text cannot be less than 3 characters.");

        RuleFor(x => x.UserProfileId)
            .NotNull().WithMessage("Userprofile id is required.")
            .NotEmpty().WithMessage("Userprofile id cannot be empty.");
    }
}