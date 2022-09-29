using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.EventBus.RabbitMQ.Enums;

namespace example.net7.api.EventHandlers;

/// <summary>
/// 测试消息类型
/// </summary>
[Rabbit("hoyo.test", EExchange.Routing, "test", "orderqueue2")]
public class TestEvent : IntegrationEvent
{
    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; } = default!;
}

/// <summary>
/// 消息处理Handler
/// </summary>
[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class TestEventHandler : IIntegrationEventHandler<TestEvent>
{
    private readonly ILogger<TestEventHandler> _logger;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="logger"></param>
    public TestEventHandler(ILogger<TestEventHandler> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// 当消息到达的时候执行的Action
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public Task HandleAsync(TestEvent @event)
    {
        _logger.LogInformation("TestEvent_{event}-----{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}
