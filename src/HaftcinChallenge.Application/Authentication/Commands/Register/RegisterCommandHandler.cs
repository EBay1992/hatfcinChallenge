using ErrorOr;
using HaftcinChallenge.Application.Authentication.Common;
using HaftcinChallenge.Application.Common.Interfaces;
using HaftcinChallenge.Domain.Common.Interfaces;
using HaftcinChallenge.Domain.Users;
using MediatR;

namespace HaftcinChallenge.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegisterResult>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOtpService _otpService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RegisterCommandHandler(
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

    public async Task<ErrorOr<RegisterResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _usersRepository.FindByMobileNumberAsync(command.MobileNumber);
        
        if (existingUser is not null)
        {
            return Error.Conflict("User.AlreadyExists", "User with this mobile number already exists. Please try logging in.");
        }
        
        var (otp, hashedOtp) = _otpService.GenerateOtp();
        
        var newUser = new User(command.MobileNumber);
        newUser.SetOtp(hashedOtp, _dateTimeProvider);

        await _usersRepository.AddAsync(newUser);
        await _unitOfWork.CommitChangesAsync();
        
        return new RegisterResult(
            newUser.Id,
            otp 
        );
    }
}