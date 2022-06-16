//using Hoyo.EventBus.Abstractions;

//namespace eventbus.subscribe.IntegrationEvents.EventHandling;

//public class OtherEventHandler : IIntegrationEventHandler<XXXXXXXXXXX>
//{
//    private readonly ILogger<OtherEventHandler> _logger;

//    public OtherEventHandler(
//       //IBasketRepository repository,
//       ILogger<OtherEventHandler> logger)
//    {
//        // _repository = repository ?? throw new ArgumentNullException(nameof(repository));
//        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//    }

//    public Task Handle(XXXXXXXXXXX @event)
//    {

//        _logger.LogInformation("----- Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

//        // await _repository.DeleteBasketAsync(@event.UserId.ToString());
//        Console.WriteLine("订阅执行");
//        return Task.CompletedTask;
//    }
//}
