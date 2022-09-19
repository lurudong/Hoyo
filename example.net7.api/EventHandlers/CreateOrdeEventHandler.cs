using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.EventBus.RabbitMQ.Enums;

namespace example.net7.api.EventHandlers;

[RabbitMQ("hoyo.test", EExchange.Routing, "test", "orderqueue2")]
public class TestEvent : IntegrationEvent
{
    public string Message { get; set; } = default!;
}

[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class TestEventHandler : IIntegrationEventHandler<TestEvent>
{
    private readonly ILogger<TestEventHandler> _logger;
    public TestEventHandler(ILogger<TestEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(TestEvent @event)
    {
        _logger.LogInformation("TestEvent_{event}-----{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}
