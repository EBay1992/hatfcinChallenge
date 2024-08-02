using HaftcinChallenge.Domain.Common.Interfaces;

namespace HaftcinChallenge.Infrastructure.Services;

public class SystemTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}