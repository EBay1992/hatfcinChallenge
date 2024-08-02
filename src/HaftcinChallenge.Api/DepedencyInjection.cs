using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

namespace HaftcinChallenge.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddHttpContextAccessor();
        services.AddSwaggerWithJwt();
        services.AddRateLimiting();

        return services;
    }
    
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HaftcinChallenge API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }
    
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("OtpRateLimit", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0, // Set to 0 to reject immediately without queuing
                        AutoReplenishment = true,
                    }));
            options.OnRejected = async (rejectedContext, cancellationToken) =>
            {
                var windowEnd = DateTimeOffset.UtcNow.AddMinutes(1);
                var retryAfter = windowEnd - DateTimeOffset.UtcNow;

                rejectedContext.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                rejectedContext.HttpContext.Response.Headers["Retry-After"] = retryAfter.TotalSeconds.ToString();
                rejectedContext.HttpContext.Response.ContentType = "application/text";
                await rejectedContext.HttpContext.Response.WriteAsync(
                    $"Rate limit exceeded. Try again in {retryAfter.TotalSeconds:F0} seconds.");
            };
        });


        return services;
    }
}