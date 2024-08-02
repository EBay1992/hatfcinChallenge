namespace HaftcinChallenge.Application.Authentication.Common;

public record LoginResult(
    Guid Id,
    string Otp);