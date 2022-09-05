﻿using CustomEvent;
using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;

namespace example.net7.api.EventHandlers;

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
