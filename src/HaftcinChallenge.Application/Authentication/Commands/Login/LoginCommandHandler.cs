using ErrorOr;
using HaftcinChallenge.Application.Authentication.Common;
using HaftcinChallenge.Application.Common.Interfaces;
using HaftcinChallenge.Domain.Common.Interfaces;
using MediatR;

namespace HaftcinChallenge.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginResult>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOtpService _otpService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LoginCommandHandler(
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork,
        IOtpService otpService, 
        IDateTimeProvider dateTimeProvider)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _otpService = otpService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<LoginResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.FindByMobileNumberAsync(command.MobileNumber);
        
        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found. Please register first.");
        }
        
        var (otp, hashedOtp) = _otpService.GenerateOtp();
        user.SetOtp(hashedOtp, _dateTimeProvider);

        await _usersRepository.UpdateAsync(user);
        await _unitOfWork.CommitChangesAsync();
        
        return new LoginResult(
            user.Id,
            otp 
        );
    }
}