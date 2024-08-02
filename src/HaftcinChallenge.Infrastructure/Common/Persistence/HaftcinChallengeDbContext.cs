using HaftcinChallenge.Application.Common.Interfaces;
using HaftcinChallenge.Domain.Users;
using HaftcinChallenge.Infrastructure.Common.Persistence.Configurations;
using HaftcinChallenge.Infrastructure.Common.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaftcinChallenge.Infrastructure.Common.Persistence;

public class HaftcinChallengeDbContext : DbContext, IUnitOfWork
{
    private readonly ILoggerFactory _loggerFactory;
    public HaftcinChallengeDbContext(DbContextOptions<HaftcinChallengeDbContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
        _loggerFactory = loggerFactory;
    }
    
    public DbSet<User> Users => Set<User>();

    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new EfCoreLoggingInterceptor(_loggerFactory.CreateLogger<EfCoreLoggingInterceptor>()));
    }
}