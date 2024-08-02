using FluentValidation;

namespace HaftcinChallenge.Application.Users.Commands;

public class CompleteProfileCommandValidator : AbstractValidator<CompleteProfileCommand>
{
    public CompleteProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z\s-]+$")
            .WithMessage("First name should only contain letters, spaces, and hyphens.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z\s-]+$")
            .WithMessage("Last name should only contain letters, spaces, and hyphens.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters.")
            .EmailAddress()
            .WithMessage("A valid email address is required.");

        RuleFor(x => x.DateOfBirth)
            .NotNull()
            .WithMessage("Date of birth is required.")
            .Must(date => date < DateTime.Now)
            .WithMessage("Date of birth must be in the past.")
            .Must(date => date > DateTime.Now.AddYears(-120))
            .WithMessage("Date of birth is not valid.");
    }
}