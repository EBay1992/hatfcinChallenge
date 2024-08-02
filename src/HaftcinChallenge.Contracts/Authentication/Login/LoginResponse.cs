namespace HaftcinChallenge.Contracts.Authentication.Login;

public record LoginResponse(
    Guid Id,
    string Otp);