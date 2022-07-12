using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Hoyo.EventBus.RabbitMQ;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEventBusRabbitMq(this IServiceCollection service, Action<RabbitMQConfig> action)
    {
        RabbitMQConfig config = new();
        action.Invoke(config);
        _ = service.AddRabbitMqPersistentConnection(config);
        _ = service.AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbitMq>(serviceProvider =>
        {
            var rabbitMqPersistentConnection = serviceProvider.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = serviceProvider.GetRequiredService<ILogger<IntegrationEventBusRabbitMq>>();
            var subsManager = serviceProvider.GetRequiredService<IIntegrationEventBusSubscriptionsManager>();
            return rabbitMqPersistentConnection is null || logger is null
                ? throw new(nameof(rabbitMqPersistentConnection))
                : new IntegrationEventBusRabbitMq(rabbitMqPersistentConnection, logger, config.RetryCount, subsManager, serviceProvider);
        });
        _ = service.AddSingleton<IIntegrationEventBusSubscriptionsManager, RabbitMqEventBusSubscriptionsManager>();
        _ = service.AddHostedService<RabbitMqIntegrationEventBusBackgroundServiceSubscribe>();
        return service;
    }

    private static IServiceCollection AddRabbitMqPersistentConnection(this IServiceCollection service, RabbitMQConfig config)
    {
        _ = service.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
            var factory = new ConnectionFactory()
            {
                HostName = config.Host,
                DispatchConsumersAsync = true,
                UserName = config.UserName,
                Password = config.PassWord,
                Port = config.Port,
            };
            return new DefaultRabbitMQPersistentConnection(factory, logger, config.RetryCount);
        });
        return service;
    }
}