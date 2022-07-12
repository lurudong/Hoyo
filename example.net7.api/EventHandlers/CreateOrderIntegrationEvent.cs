using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using System.Text.Json;

namespace example.net7.api.EventHandlers;

[RabbitMQ("hoyo.rabbitmqbus.test", ExchangeType.Routing, "createorder", "testqueue")]
public class CreateOrderIntegrationEvent : IntegrationEvent
{
    public string OrderNo { get; set; } = default!;
}

[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class CreateOrderIntegrationHandler : IIntegrationEventHandler<CreateOrderIntegrationEvent>
{

    private readonly ILogger<CreateOrderIntegrationHandler> _logger;

    public CreateOrderIntegrationHandler(ILogger<CreateOrderIntegrationHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(CreateOrderIntegrationEvent @event)
    {

        _logger.LogInformation("CreateOrderIntegrationEvent_{event}-----{date}", JsonSerializer.Serialize(@event), DateTime.Now);
        return Task.CompletedTask;
    }
}
