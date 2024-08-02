using ErrorOr;
using HaftcinChallenge.Application.Authentication.Common;
using MediatR;

namespace HaftcinChallenge.Application.Authentication.Commands.Login;

public record LoginCommand(
    string MobileNumber) : IRequest<ErrorOr<LoginResult>>;