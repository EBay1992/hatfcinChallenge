using HaftcinChallenge.Domain.Users;

namespace HaftcinChallenge.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}