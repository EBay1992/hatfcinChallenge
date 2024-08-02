namespace HaftcinChallenge.Contracts.Users.ListUser;

public record ListUsersResponse(
    IEnumerable<UserDto> Users,
    int TotalCount,
    int Page,
    int PageSize);

public record UserDto(
    string FirstName,
    string LastName,
    string Email,
    string MobileNumber,
    DateTime? DateOfBirth);