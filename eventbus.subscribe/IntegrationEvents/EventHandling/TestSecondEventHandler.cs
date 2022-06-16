using eventbus.events;
using Hoyo.EventBus;
using Hoyo.EventBus.Abstractions;

namespace eventbus.subscribe.IntegrationEvents.EventHandling;

[HoyoSubscribe("second.publish.test")]
public class TestSecondEventHandler : IIntegrationEventHandler<TestSecondEvent>
{
    private readonly ILogger<TestSecondEventHandler> _logger;

    public TestSecondEventHandler(ILogger<TestSecondEventHandler> logger)
    {
        // _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Handle(TestSecondEvent @event)
    {

        _logger.LogInformation("-SecondEvent- Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // await _repository.DeleteBasketAsync(@event.UserId.ToString());
        Console.WriteLine("订阅执行");
        return Task.CompletedTask;
    }
}