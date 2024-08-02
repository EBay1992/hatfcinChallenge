namespace HaftcinChallenge.Application.Authentication.Common;

public record RegisterResult(
    Guid Id,
    string Otp);