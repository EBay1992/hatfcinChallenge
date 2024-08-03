using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HaftcinChallenge.Application.Authentication.Common;
using HaftcinChallenge.Contracts.Authentication.Login;
using HaftcinChallenge.Contracts.Authentication.Register;
using HaftcinChallenge.Contracts.Authentication.VerifyOtp;
using HaftcinChallenge.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HaftcinChallenge.IntegrationTests.Api.Controller.Tests;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable, IAsyncDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly string _databaseName = Guid.NewGuid().ToString();

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<HaftcinChallengeDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<HaftcinChallengeDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                });
            });
        });
    }

    [Theory]
    [InlineData("09123456799")] // Valid mobile number
    public async Task Register_WithValidMobileNumber_ShouldReturnOk(string mobileNumber)
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new RegisterRequest(mobileNumber);

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<RegisterResponse>();
        content.Should().NotBeNull();
        content!.Id.Should().NotBeEmpty();
        content.Otp.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("1234567890")] // Invalid mobile number
    [InlineData("")] // Empty string
    [InlineData("abc123")] // Non-numeric string
    public async Task Register_WithInvalidMobileNumber_ShouldReturnBadRequest(string mobileNumber)
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new RegisterRequest(mobileNumber);

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("09123456799")] // Valid mobile number
    public async Task Login_WithValidMobileNumber_ShouldReturnOk(string mobileNumber)
    {
        // Arrange
        var client = _factory.CreateClient();
        var result = await RegisterUserAsync(client, mobileNumber);
        var request = new LoginRequest(mobileNumber);

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
        content.Should().NotBeNull();
        content!.Otp.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("1234567890")] // Invalid mobile number
    [InlineData("")] // Empty string
    [InlineData("abc123")] // Non-numeric string
    public async Task Login_WithInvalidMobileNumber_ShouldReturnBadRequest(string mobileNumber)
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new LoginRequest(mobileNumber);

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task VerifyOtp_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        var mobileNumber = "09123458967";
        var (userId, _) = await RegisterUserAsync(client, mobileNumber);
        var otp = await LoginUserAsync(client, mobileNumber);
        var request = new VerifyOtpRequest(otp);

        // Act
        var response = await client.PostAsJsonAsync($"/api/auth/{userId}/verify-otp", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<VerifyOtpResult>();
        content.Should().NotBeNull();
        content!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task VerifyOtp_WithInvalidOtp_ShouldReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var mobileNumber = "09123456799";
        var (userId, _) = await RegisterUserAsync(client, mobileNumber);
        await LoginUserAsync(client, mobileNumber);
        var request = new VerifyOtpRequest("invalid_otp");

        // Act
        var response = await client.PostAsJsonAsync($"/api/auth/{userId}/verify-otp", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task<(Guid UserId, string Otp)> RegisterUserAsync(HttpClient client, string mobileNumber)
    {
        var registerRequest = new RegisterRequest(mobileNumber);
        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", registerRequest);
        registerResponse.EnsureSuccessStatusCode();
        var content = await registerResponse.Content.ReadFromJsonAsync<RegisterResponse>();
        return (content!.Id, content.Otp);
    }

    private async Task<string> LoginUserAsync(HttpClient client, string mobileNumber)
    {
        var loginRequest = new LoginRequest(mobileNumber);
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();
        var content = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        return content!.Otp;
    }

    public void Dispose()
    {
        _factory.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}
