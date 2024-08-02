using ErrorOr;
using HaftcinChallenge.Application.Users.Common;
using MediatR;

namespace HaftcinChallenge.Application.Users.Queries;

public record ListUsersQuery(
    string? SearchTerm,
    int Page = 1,
    int PageSize = 10) : IRequest<ErrorOr<ListUsersResult>>;