using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hoyo.EventBus.RabbitMQ;

/// <summary>
/// 后台任务进行事件订阅
/// </summary>
internal class RabbitMQSubscribeService : BackgroundService
{
    private readonly IServiceProvider _rootServiceProvider;

    public RabbitMQSubscribeService(IServiceProvider serviceProvider)
    {
        _rootServiceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _rootServiceProvider.CreateScope();
        var eventBus = scope.ServiceProvider.GetService<IIntegrationEventBus>();
        if (eventBus is null) throw new("RabbitMQ集成事件总线没有注册");
        eventBus.Subscribe();
        await Task.CompletedTask;
    }
}