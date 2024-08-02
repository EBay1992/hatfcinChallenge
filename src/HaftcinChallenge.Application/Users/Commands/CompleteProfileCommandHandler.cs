using ErrorOr;
using HaftcinChallenge.Application.Common.Interfaces;
using HaftcinChallenge.Application.Users.Common;
using MediatR;

namespace HaftcinChallenge.Application.Users.Commands;

public class CompleteProfileCommandHandler : IRequestHandler<CompleteProfileCommand, ErrorOr<CompleteProfileResult>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteProfileCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<CompleteProfileResult>> Handle(CompleteProfileCommand command, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(command.Id);
        if (user is null)
        {
            return Error.NotFound("User not found");
        }

        user.UpdateProfile(command.FirstName, command.LastName, command.Email, command.DateOfBirth);
        await _usersRepository.UpdateAsync(user);
        await _unitOfWork.CommitChangesAsync();

        return new CompleteProfileResult(user.Id, user.FirstName, user.LastName, user.Email, user.DateOfBirth);
    }
}