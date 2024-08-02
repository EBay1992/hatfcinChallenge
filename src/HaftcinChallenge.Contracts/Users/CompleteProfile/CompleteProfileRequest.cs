namespace HaftcinChallenge.Contracts.Users.CompleteProfile;

public record CompleteProfileRequest(
    string FirstName,
    string LastName,
    string Email,
    string DateOfBirth);