using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace HaftcinChallenge.Infrastructure.Common.Persistence.Interceptors;

public class EfCoreLoggingInterceptor : DbCommandInterceptor
{
    private readonly ILogger<EfCoreLoggingInterceptor> _logger;

    public EfCoreLoggingInterceptor(ILogger<EfCoreLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader> result, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing DB Command: {CommandText}", command.CommandText);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing Non-Query DB Command: {CommandText}", command.CommandText);
        return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }
}