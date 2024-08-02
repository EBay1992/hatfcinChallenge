using System.Text;
using HaftcinChallenge.Application.Common.Interfaces;
using HaftcinChallenge.Domain.Common.Interfaces;
using HaftcinChallenge.Infrastructure.Authentication.Services;
using HaftcinChallenge.Infrastructure.Authentication.TokenGenerator;
using HaftcinChallenge.Infrastructure.Common.Persistence;
using HaftcinChallenge.Infrastructure.Services;
using HaftcinChallenge.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HaftcinChallenge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddAuthentication(configuration)
            .AddOtpSetting(configuration)
            .AddPersistence(configuration)
            .AddSingleton<IDateTimeProvider, SystemTimeProvider>();
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HaftcinChallengeDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine)
                // .EnableSensitiveDataLogging()
            );
        
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<HaftcinChallengeDbContext>());

        return services;
    }

    public static IServiceCollection AddOtpSetting(this IServiceCollection services, IConfiguration configuration)
    {
        var otpSettings = configuration.GetSection(OtpSettings.Section).Get<OtpSettings>();

        if (otpSettings == null || string.IsNullOrWhiteSpace(otpSettings.SecretKey))
        {
            throw new InvalidOperationException("OTP secret key is not configured.");
        }

        services.AddSingleton<IOtpService>(provider => new OtpService(otpSettings.SecretKey));

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.Section, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });


        return services;
    }
}