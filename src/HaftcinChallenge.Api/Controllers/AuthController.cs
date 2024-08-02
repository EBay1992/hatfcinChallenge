using HaftcinChallenge.Application.Authentication.Commands.Login;
using HaftcinChallenge.Application.Authentication.Commands.Register;
using HaftcinChallenge.Application.Authentication.Commands.VerifyOtp;
using HaftcinChallenge.Contracts.Authentication.Login;
using HaftcinChallenge.Contracts.Authentication.Register;
using HaftcinChallenge.Contracts.Authentication.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HaftcinChallenge.Api.Controllers;

[Route("api/auth")]
[EnableRateLimiting("OtpRateLimit")]
[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ISender mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        _logger.LogInformation("Attempting to register user with mobile number: {MobileNumber}", request.MobileNumber);

        var command = new RegisterCommand(request.MobileNumber);

        var authResult = await _mediator.Send(command);

        return authResult.Match(
            registerResult => {
                _logger.LogInformation("User registered successfully with ID: {UserId}", registerResult.Id);
                return base.Ok(registerResult);
            },
            errors => {
                _logger.LogWarning("Registration failed for mobile number: {MobileNumber}. Errors: {@Errors}", request.MobileNumber, errors);
                return Problem(errors);
            });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        _logger.LogInformation("Attempting login for mobile number: {MobileNumber}", request.MobileNumber);

        var command = new LoginCommand(request.MobileNumber);

        var authResult = await _mediator.Send(command);

        return authResult.Match(
            loginResult => {
                _logger.LogInformation("Login successful for user with ID: {UserId}", loginResult.Id);
                return base.Ok(loginResult);
            },
            errors => {
                _logger.LogWarning("Login failed for mobile number: {MobileNumber}. Errors: {@Errors}", request.MobileNumber, errors);
                return Problem(errors);
            });
    }
    
    [HttpPost("{id:guid}/verify-otp")]
    public async Task<IActionResult> VerifyOtp(Guid id, VerifyOtpRequest request)
    {
        _logger.LogInformation("Attempting to verify OTP for user ID: {UserId}", id);

        var command = new VerifyOtpCommand(id, request.Otp);

        var authResult = await _mediator.Send(command);

        return authResult.Match(
            verifyOtpResult => {
                _logger.LogInformation("OTP verification successful for user ID: {UserId}", id);
                return Ok(verifyOtpResult);
            },
            errors => {
                _logger.LogWarning("OTP verification failed for user ID: {UserId}. Errors: {@Errors}", id, errors);
                return Problem(errors);
            });
    }
}
