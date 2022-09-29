using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.EventBus.RabbitMQ.Enums;

namespace example.net7.api.EventHandlers;

/// <summary>
/// 延时消息测试事件
/// </summary>
[Rabbit(exchange: "delayedmessage2", exchangeType: EExchange.Delayed, routingKey: "delay2", queue: "testdelay2", enable: true)]
[RabbitHeader("x-delay", 5000)]
public class DelayedMessageEvent2 : IntegrationEvent
{
    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; } = default!;
}

/// <summary>
/// 延时消息处理器
/// </summary>
[DependencyInjection(ServiceLifetime.Transient, AddSelf = true)]
public class DelayedMessageEventHandler2 : IIntegrationEventHandler<DelayedMessageEvent2>
{
    private readonly ILogger<DelayedMessageEventHandler2> _logger;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="logger"></param>
    public DelayedMessageEventHandler2(ILogger<DelayedMessageEventHandler2> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// 当消息到达后执行的action
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public Task HandleAsync(DelayedMessageEvent2 @event)
    {
        _logger.LogInformation("DelayedMessageEventEvent2_{event}-----{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}