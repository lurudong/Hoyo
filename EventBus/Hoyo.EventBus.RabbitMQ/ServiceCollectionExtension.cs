using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Hoyo.EventBus.RabbitMQ;

public static class ServiceCollectionExtension
{
    public static void AddEventBusRabbitMQ(this IServiceCollection service, Action<RabbitMQConfig>? action = null)
    {
        RabbitMQConfig config = new();
        action?.Invoke(config);
        _ = service.AddRabbitMQPersistentConnection(config)
            .AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbitMQ>(sp =>
            {
                var rabbitmqconn = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<IntegrationEventBusRabbitMQ>>();
                var subsManager = sp.GetRequiredService<ISubscriptionsManager>();
                return rabbitmqconn is null || logger is null
                    ? throw new(nameof(rabbitmqconn))
                    : new IntegrationEventBusRabbitMQ(rabbitmqconn, logger, config.RetryCount, subsManager, sp);
            })
            .AddSingleton<ISubscriptionsManager, RabbitMQSubscriptionsManager>()
            .AddHostedService<RabbitMQSubscribeService>();
    }

    private static IServiceCollection AddRabbitMQPersistentConnection(this IServiceCollection service, RabbitMQConfig config)
    {
        _ = service.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
            return new RabbitMQPersistentConnection(new ConnectionFactory()
            {
                HostName = config.Host,
                DispatchConsumersAsync = true,
                UserName = config.UserName,
                Password = config.PassWord,
                Port = config.Port,
                VirtualHost = config.VirtualHost
            }, logger, config.RetryCount);
        });
        return service;
    }
}