using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Hoyo.EventBus.RabbitMQ;

public static class ServiceCollectionExtension
{
    public static void AddEventBusRabbit(this IServiceCollection service, Action<RabbitConfig>? action = null)
    {
        RabbitConfig config = new();
        action?.Invoke(config);
        _ = service.AddRabbitPersistentConnection(config)
            .AddSingleton<IIntegrationEventBus, IntegrationEventBusRabbit>(sp =>
            {
                var rabbitConn = sp.GetRequiredService<IRabbitPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<IntegrationEventBusRabbit>>();
                var subsManager = sp.GetRequiredService<ISubscriptionsManager>();
                return rabbitConn is null
                    ? throw new(nameof(rabbitConn))
                    : new IntegrationEventBusRabbit(rabbitConn, logger, config.RetryCount, subsManager, sp);
            })
            .AddSingleton<ISubscriptionsManager, RabbitSubscriptionsManager>()
            .AddHostedService<RabbitSubscribeService>();
    }

    private static IServiceCollection AddRabbitPersistentConnection(this IServiceCollection service, RabbitConfig config)
    {
        _ = service.AddSingleton<IRabbitPersistentConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitPersistentConnection>>();
            return new RabbitPersistentConnection(new ConnectionFactory()
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