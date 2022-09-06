using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.EventBus.RabbitMQ.Enums;

namespace example.net7.api.EventHandlers;

[RabbitMQ(exchange: "delayedmessage2", exchangeType: EExchange.Delayed, routingKey: "delay2", queue: "testdelay2")]
[RabbitMQHeader("x-delay", 5000)]
public class DelayedMessageEvent2 : IntegrationEvent
{
    public string Message { get; set; } = default!;
}

[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class DelayedMessageEventHandler2 : IIntegrationEventHandler<DelayedMessageEvent2>
{
    private readonly ILogger<DelayedMessageEventHandler2> _logger;

    public DelayedMessageEventHandler2(ILogger<DelayedMessageEventHandler2> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(DelayedMessageEvent2 @event)
    {
        _logger.LogInformation("DelayedMessageEventEvent2_{event}-----{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}