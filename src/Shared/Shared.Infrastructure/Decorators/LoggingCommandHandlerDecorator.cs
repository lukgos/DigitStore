using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Shared.Abstractions.CQRS;

namespace Shared.Infrastructure.Decorators;

public sealed class LoggingCommandHandlerDecorator<TCommand>(ICommandHandler<TCommand> innerHandler, ILogger<LoggingCommandHandlerDecorator<TCommand>> logger) 
    : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken ct)
    {
        var commandName = typeof(TCommand).Name;
        logger.LogInformation("Started handling command: {CommandName}", commandName);
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            await innerHandler.HandleAsync(command, ct);
            
            stopwatch.Stop();
            logger.LogInformation("Completed handling command: {CommandName} in {StopwatchElapsedMilliseconds}ms", commandName, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();
            logger.LogError(exception, "Failed handling command: {CommandName}. Time elapsed: {StopwatchElapsedMilliseconds}ms", commandName, stopwatch.ElapsedMilliseconds);
            throw; 
        }
    }
}