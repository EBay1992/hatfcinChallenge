namespace HaftcinChallenge.Application.Users.Common;

public record CompleteProfileResult(Guid UserId, string FirstName, string LastName, string Email, DateTime DateOfBirth);