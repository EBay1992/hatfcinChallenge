using FluentValidation;

namespace HaftcinChallenge.Application.Authentication.Commands.VerifyOtp;

public class VerifyOtpValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpValidator()
    {
        RuleFor(x => x.Otp)
            .NotEmpty()
            .WithMessage("Otp is required.")
            .Matches(@"^\d{6}$")
            .WithMessage("Otp must be a 6-digit number.");
    }
}