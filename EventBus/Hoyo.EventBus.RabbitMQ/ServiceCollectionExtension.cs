using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Hoyo.EventBus.RabbitMQ;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEventBusRabbitMQ(this IServiceCollection service, Action<RabbitMQConfig> action)
    {
        RabbitMQConfig config = new();
        action.Invoke(config);
        _ = service.AddRabbitMQPersistentConnection(config);
        _ = service.AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbitMQ>(serviceProvider =>
        {
            var rabbitMQPersistentConnection = serviceProvider.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = serviceProvider.GetRequiredService<ILogger<IntegrationEventBusRabbitMQ>>();
            var subsManager = serviceProvider.GetRequiredService<IIntegrationEventBusSubscriptionsManager>();
            return rabbitMQPersistentConnection is null || logger is null
                ? throw new(nameof(rabbitMQPersistentConnection))
                : new IntegrationEventBusRabbitMQ(rabbitMQPersistentConnection, logger, config.RetryCount, subsManager, serviceProvider);
        });
        _ = service.AddSingleton<IIntegrationEventBusSubscriptionsManager, RabbitMQEventBusSubscriptionsManager>();
        _ = service.AddHostedService<RabbitMQIntegrationEventBusBackgroundServiceSubscribe>();
        return service;
    }

    private static IServiceCollection AddRabbitMQPersistentConnection(this IServiceCollection service, RabbitMQConfig config)
    {
        _ = service.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
            var factory = new ConnectionFactory()
            {
                HostName = config.Host,
                DispatchConsumersAsync = true,
                UserName = config.UserName,
                Password = config.PassWord,
                Port = config.Port,
                VirtualHost = config.VirtualHost
            };
            return new RabbitMQPersistentConnection(factory, logger, config.RetryCount);
        });
        return service;
    }
}