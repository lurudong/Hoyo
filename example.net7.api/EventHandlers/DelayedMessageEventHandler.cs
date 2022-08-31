using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.EventBus.RabbitMQ.Enums;

namespace example.net7.api.EventHandlers;

[RabbitMQ(exchange: "hoyo.rabbitmqbus.delayedmessage", exchangeType: EExchange.DelayedMessage, routingKey: "delay", queue: "testdelay")]

public class DelayedMessageEvent : IntegrationEvent
{
    public string Message { get; set; } = default!;
}

[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class DelayedMessageEventHandler : IIntegrationEventHandler<DelayedMessageEvent>
{
    private readonly ILogger<DelayedMessageEventHandler> _logger;

    public DelayedMessageEventHandler(ILogger<DelayedMessageEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(DelayedMessageEvent @event)
    {
        _logger.LogInformation("DelayedMessageEventEvent_{event}-----{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}