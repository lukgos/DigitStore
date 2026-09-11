using Account.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Customer.Module.Consumers;

public class AccountCreatedConsumer(ILogger<AccountCreatedConsumer> logger) : IConsumer<AccountCreated>
{
    public Task Consume(ConsumeContext<AccountCreated> context)
    {
        logger.LogInformation($"Profile created for {context.Message.UserId} with {context.Message.Email}");
        
        return Task.CompletedTask;
    }
}