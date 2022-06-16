using eventbus.events;
using Hoyo.EventBus;
using Hoyo.EventBus.Abstractions;

namespace eventbus.subscribe.IntegrationEvents.EventHandling;

[HoyoSubscribe("order.publish.test")]
public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    private readonly ILogger<OrderStartedIntegrationEventHandler> _logger;

    public OrderStartedIntegrationEventHandler(
       //IBasketRepository repository,
       ILogger<OrderStartedIntegrationEventHandler> logger)
    {
        // _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Handle(OrderStartedIntegrationEvent @event)
    {

        _logger.LogInformation("----- Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // await _repository.DeleteBasketAsync(@event.UserId.ToString());
        Console.WriteLine("订阅执行");
        return Task.CompletedTask;
    }
}
