using System.Globalization;
using HaftcinChallenge.Application.Common.Interfaces;
using HaftcinChallenge.Domain.Users;
using HaftcinChallenge.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HaftcinChallenge.Infrastructure.Users;

public class UsersRepository(HaftcinChallengeDbContext dbContext) : IUsersRepository
{
    public async Task AddAsync(User user)
    {
        await dbContext.AddAsync(user);
    }

    public async Task<bool> ExistsByMobileNumberAsync(string mobileNumber)
    {
        return await dbContext.Users
            .AnyAsync(u => 
                u.MobileNumber == mobileNumber);
    }
    
    public async Task<User?> FindByMobileNumberAsync(string mobileNumber)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);
    }
    
    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await dbContext.Users
            .FindAsync(userId);
    }

    public Task UpdateAsync(User user)
    {
        dbContext.Update(user);
        return Task.CompletedTask;
    }
    
    public async Task<(List<User> Users, int TotalCount)> SearchUsersAsync(string? searchTerm, int page, int pageSize)
    {
        if (pageSize > 50)
        {
            pageSize = 50;
        }

        var query = dbContext.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLower = searchTerm.ToLower();

            if (DateTime.TryParseExact(searchTermLower, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                query = query.Where(u => 
                    EF.Functions.DateDiffDay(u.DateOfBirth.Date, parsedDate.Date) == 0);
            }
            else
            {
                query = query.Where(u =>
                    u.MobileNumber.Contains(searchTermLower) ||
                    u.FirstName.Contains(searchTermLower) ||
                    u.LastName.Contains(searchTermLower) ||
                    u.Email!.Contains(searchTermLower));
            }
        }

        var totalCount = await query.CountAsync();
        var users = await query
            .OrderBy(u => u.LastName) // Add ordering to ensure consistent results
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }
}
