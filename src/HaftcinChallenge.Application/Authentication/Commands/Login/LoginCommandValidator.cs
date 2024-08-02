using FluentValidation;

namespace HaftcinChallenge.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .WithMessage("Mobile number is required.")
            .Matches(@"^(\+98|0)?9\d{9}$")
            .WithMessage("Invalid phone number format. Please use a valid mobile number.");
    }
}