namespace HaftcinChallenge.Contracts.Users.ListUser;

public record ListUsersRequest(string? SearchTerm = "", int Page = 1, int PageSize = 10);