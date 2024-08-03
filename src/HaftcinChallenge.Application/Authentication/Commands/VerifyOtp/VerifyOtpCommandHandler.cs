using ErrorOr;
using HaftcinChallenge.Application.Authentication.Common;
using HaftcinChallenge.Application.Common.Interfaces;
using HaftcinChallenge.Domain.Common.Interfaces;
using MediatR;

namespace HaftcinChallenge.Application.Authentication.Commands.VerifyOtp;

public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, ErrorOr<VerifyOtpResult>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IOtpService _otpService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public VerifyOtpCommandHandler(
        IUsersRepository usersRepository,
        IOtpService otpService,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork, 
        IDateTimeProvider dateTimeProvider)
    {
        _usersRepository = usersRepository;
        _otpService = otpService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<VerifyOtpResult>> Handle(VerifyOtpCommand command, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(command.Id);
        
        if (user is null)
        {
            return Error.NotFound(description: "User not found");
        }

        if (user.IsOtpExpired(_dateTimeProvider))
        {
            user.RemoveExpiredOtp();
            await _unitOfWork.CommitChangesAsync();
            return Error.Conflict("OtpExpired", "OTP has expired. Please request a new one.");
        }

        if (!_otpService.VerifyOtp(user.OtpHash!, command.Otp))
        {
            return Error.Validation(description: "Invalid OTP");
        }

        user.VerifyUser();
        await _unitOfWork.CommitChangesAsync();

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new VerifyOtpResult(token);
    }
}