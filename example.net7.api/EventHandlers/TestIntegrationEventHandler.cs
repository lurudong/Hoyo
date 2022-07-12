using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;

namespace example.net7.api.EventHandlers;

[RabbitMQ("hoyo.mqtest.002", ExchangeType.Routing, "test_0001", "testqueue1")]
public class TestIntegrationEvent : IntegrationEvent
{
    public string Name { get; set; } = "";
}

[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
{
    private readonly ILogger<TestIntegrationEventHandler> _logger;

    public TestIntegrationEventHandler(ILogger<TestIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(TestIntegrationEvent @event)
    {
        _logger.LogInformation("{name}------{date}", @event.Name, DateTime.Now);
        return Task.CompletedTask;
    }
}
