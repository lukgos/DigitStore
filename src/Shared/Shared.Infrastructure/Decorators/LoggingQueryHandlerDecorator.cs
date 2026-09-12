using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Shared.Abstractions.CQRS;

namespace Shared.Infrastructure.Decorators;

public sealed class LoggingQueryHandlerDecorator<TQuery, TResult>(IQueryHandler<TQuery, TResult> innerHandler, ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger) 
    : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken ct)
    {
        var queryName = typeof(TQuery).Name;
        logger.LogInformation("Started handling query: {QueryName}", queryName);
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var result = await innerHandler.HandleAsync(query, ct);
            
            stopwatch.Stop();
            logger.LogInformation("Completed handling query: {QueryName} in {ElapsedMilliseconds}ms", queryName, stopwatch.ElapsedMilliseconds);
                
            return result;
        }
        catch (Exception exception)
        {
            stopwatch.Stop();
            logger.LogError(exception, "Failed handling query: {QueryName}. Time elapsed: {ElapsedMilliseconds}ms", queryName, stopwatch.ElapsedMilliseconds);
            throw; 
        }
    }
}