using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using Polly.Caching;
using RabbitMQ.Client;
using System.Linq.Expressions;
using ExchangeType = Hoyo.EventBus.RabbitMQ.Attributes.ExchangeType;

namespace example.net7.api.EventHandlers;

[RabbitMQ(exchange: "hoyo.rabbitmqbus.delayedmessage", exchangeType: ExchangeType.DelayedMessage, routingKey: "delay", queue: "testdelay")]

[RabbitMQHeader("x-delay", 5000)]
[RabbitMQArg("x-delayed-type", "direct")]
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