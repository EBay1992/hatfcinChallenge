namespace HaftcinChallenge.Contracts.Authentication.Register;

public record RegisterResponse(
    Guid Id,
    string Otp);