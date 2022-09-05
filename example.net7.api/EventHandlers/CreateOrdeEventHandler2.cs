using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;

namespace example.net7.api.EventHandlers;

[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class CreateOrderEventHandler2 : IIntegrationEventHandler<CreateOrderEvent>
{
    private readonly ILogger<CreateOrderEventHandler2> _logger;
    public CreateOrderEventHandler2(ILogger<CreateOrderEventHandler2> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(CreateOrderEvent @event)
    {
        _logger.LogInformation("CreateOrderIntegrationEvent2_{event}-----{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}
