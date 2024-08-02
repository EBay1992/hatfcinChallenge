using HaftcinChallenge.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace HaftcinChallenge.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string MobileNumber) : IRequest<ErrorOr<RegisterResult>>;