using HaftcinChallenge.Application.Users.Common;
using MediatR;
using ErrorOr;

namespace HaftcinChallenge.Application.Users.Commands;

public record CompleteProfileCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth) : IRequest<ErrorOr<CompleteProfileResult>>;
