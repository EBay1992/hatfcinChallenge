using HaftcinChallenge.Domain.Users;

namespace HaftcinChallenge.Application.Users.Common;

public record ListUsersResult(
    IEnumerable<User> Users,
    int TotalCount,
    int Page,
    int PageSize);