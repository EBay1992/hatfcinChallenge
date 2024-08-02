using HaftcinChallenge.Domain.Users;

namespace HaftcinChallenge.Application.Common.Interfaces;

public interface IUsersRepository
{
    Task AddAsync(User user);
    Task<User?> FindByMobileNumberAsync(string mobileNumber);
    Task<User?> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
    Task<(List<User> Users, int TotalCount)> SearchUsersAsync(
        string? querySearchTerm,
        int queryPage,
        int queryPageSize);
}