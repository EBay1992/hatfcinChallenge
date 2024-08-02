using HaftcinChallenge.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HaftcinChallenge.Infrastructure.Common.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.MobileNumber).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(50);
        builder.Property(u => u.LastName).HasMaxLength(50);
        builder.Property(u => u.Email).HasMaxLength(100);
        
        // indexes
        builder.HasIndex(u => u.Email);
        builder.HasIndex(u => u.FirstName);
        builder.HasIndex(u => u.LastName);
        builder.HasIndex(u => u.MobileNumber);
        builder.HasIndex(u => u.DateOfBirth);



        // Seed data
        builder.HasData(
            new User("09123456789",
                "John",
                "Doe",
                "john.doe@example.com",
                new DateTime(1990,
                    1,
                    1),
                Guid.Parse("11111111-1111-1111-1111-111111111111")),
            new User("09234567890",
                "Jane",
                "Smith",
                "jane.smith@example.com",
                new DateTime(1985,
                    5,
                    15),
                Guid.Parse("22222222-2222-2222-2222-222222222222")),
            new User("09345678901",
                "Alice",
                "Johnson",
                "alice.johnson@example.com",
                new DateTime(1992,
                    9,
                    30),
                Guid.Parse("33333333-3333-3333-3333-333333333333")),
            new User("09456789012",
                "Bob",
                "Williams",
                "bob.williams@example.com",
                new DateTime(1988,
                    3,
                    20),
                Guid.Parse("44444444-4444-4444-4444-444444444444")),
            new User("09567890123",
                "Charlie",
                "Brown",
                "charlie.brown@example.com",
                new DateTime(1995,
                    7,
                    10),
                Guid.Parse("55555555-5555-5555-5555-555555555555"))
        );
    }
}