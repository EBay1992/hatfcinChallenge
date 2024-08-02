using HaftcinChallenge.Application.Users.Commands;
using HaftcinChallenge.Application.Users.Queries;
using HaftcinChallenge.Contracts.Users.CompleteProfile;
using HaftcinChallenge.Contracts.Users.ListUser;
using HaftcinChallenge.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaftcinChallenge.Api.Controllers;

[Route("api/users/")]
[ApiController]
[Authorize]
public class UsersController : ApiController
{
    private readonly ISender _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ISender mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPut("{id:guid}/complete-profile")]
    public async Task<IActionResult> CompleteProfile(Guid id, CompleteProfileRequest request)
    {
        _logger.LogInformation("Attempting to complete profile for user {UserId}", id);
        
        var parsedResult = base.ParseBirthDate(request.DateOfBirth);
        if (parsedResult.IsError)
        {
            _logger.LogWarning("Invalid birth date format for user {UserId}: {DateOfBirth}", id, request.DateOfBirth);
            return Problem(parsedResult.Errors);
        }
        
        var completeProfileCommand = new CompleteProfileCommand(
            id,
            request.FirstName,
            request.LastName, 
            request.Email,
            parsedResult.Value);
        
        var result = await _mediator.Send(completeProfileCommand);

        return result.Match(
            profile => {
                _logger.LogInformation("Profile completed successfully for user {UserId}", id);
                return Ok(profile);
            },
            errors => {
                _logger.LogError("Failed to complete profile for user {UserId}. Errors: {@Errors}", id, errors);
                return Problem(errors);
            });
    }
    
    [HttpGet]
    public async Task<IActionResult> ListUsers([FromQuery] string? searchTerm = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Listing users with searchTerm: {SearchTerm}, page: {Page}, pageSize: {PageSize}", searchTerm, page, pageSize);
        
        var query = new ListUsersQuery(searchTerm, page, pageSize);
        var result = await _mediator.Send(query);
        
        return result.Match(
            usersResult =>
            {
                var mapperUsersResult = MapUserToUserDto(usersResult.Users);
                _logger.LogInformation("Successfully retrieved {UserCount} users", mapperUsersResult.Count());
                return Ok(new ListUsersResponse(mapperUsersResult, usersResult.TotalCount, usersResult.Page, usersResult.PageSize));
            },
            errors => {
                _logger.LogError("Failed to list users. Errors: {@Errors}", errors);
                return Problem(errors);
            });
    }

    private IEnumerable<UserDto> MapUserToUserDto(IEnumerable<User> users)
    {
        var userDtos = users.Select(u => new UserDto(
            u.FirstName,
            u.LastName,
            u.Email,
            u.MobileNumber,
            u.DateOfBirth)
        );

        return userDtos;
    }
}
