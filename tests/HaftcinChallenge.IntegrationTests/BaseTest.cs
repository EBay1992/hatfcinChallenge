using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HaftcinChallenge.Infrastructure.Common.Persistence;

namespace HaftcinChallenge.IntegrationTests;

public class TestBase
{
    protected WebApplicationFactory<Program> Factory { get; }

    public TestBase()
    {
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the app's HaftcinChallengeDbContext
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<HaftcinChallengeDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    //substitute in-memory database for testing
                    services.AddDbContext<HaftcinChallengeDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });
        
        
    }
}