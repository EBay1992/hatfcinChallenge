using HaftcinChallenge.Application.Common.Interfaces;
using HaftcinChallenge.Application.Users.Common;
using MediatR;
using ErrorOr;

namespace HaftcinChallenge.Application.Users.Queries;

public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, ErrorOr<ListUsersResult>>
{
    private readonly IUsersRepository _usersRepository;

    public ListUsersQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<ErrorOr<ListUsersResult>> Handle(ListUsersQuery query, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _usersRepository.SearchUsersAsync(
            querySearchTerm: query.SearchTerm,
            queryPage: query.Page,
            query.PageSize);

        return new ListUsersResult(users, totalCount, query.Page, query.PageSize);
    }
}