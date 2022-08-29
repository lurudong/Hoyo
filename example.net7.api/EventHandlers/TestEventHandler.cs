using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;

namespace example.net7.api.EventHandlers;

[RabbitMQ("hoyo.mqtest.002", ExchangeType.Routing, "test", "testqueue")]
public class TestEvent : IntegrationEvent
{
    public string Message { get; set; } = "";
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
        _logger.LogInformation("{name}------{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}
