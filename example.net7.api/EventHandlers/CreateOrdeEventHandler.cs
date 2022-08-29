﻿using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;

namespace example.net7.api.EventHandlers;

[RabbitMQ("hoyo.rabbitmqbus.test", ExchangeType.Routing, "createorder", "testqueue")]
public class CreateOrderEvent : IntegrationEvent
{
    public string Message { get; set; } = default!;
}

[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class CreateOrderEventHandler : IIntegrationEventHandler<CreateOrderEvent>
{

    private readonly ILogger<CreateOrderEventHandler> _logger;

    public CreateOrderEventHandler(ILogger<CreateOrderEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(CreateOrderEvent @event)
    {
        _logger.LogInformation("CreateOrderIntegrationEvent_{event}-----{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}
