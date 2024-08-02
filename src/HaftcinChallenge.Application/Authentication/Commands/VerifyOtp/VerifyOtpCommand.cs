using HaftcinChallenge.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace HaftcinChallenge.Application.Authentication.Commands.VerifyOtp;

public record VerifyOtpCommand(Guid Id, string Otp) : IRequest<ErrorOr<VerifyOtpResult>>;