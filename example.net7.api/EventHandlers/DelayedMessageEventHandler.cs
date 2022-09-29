using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.EventBus.RabbitMQ.Enums;

namespace example.net7.api.EventHandlers;

/// <summary>
/// 延时消息测试事件
/// </summary>
[Rabbit(exchange: "delayedtest", exchangeType: EExchange.Delayed, routingKey: "delay", queue: "testdelay")]
[RabbitArg("x-delayed-type", "topic")]
public class DelayedMessageEvent : IntegrationEvent
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
public class DelayedMessageEventHandler : IIntegrationEventHandler<DelayedMessageEvent>
{
    private readonly ILogger<DelayedMessageEventHandler> _logger;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="logger"></param>
    public DelayedMessageEventHandler(ILogger<DelayedMessageEventHandler> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// 当消息到达时执行的Action
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public Task HandleAsync(DelayedMessageEvent @event)
    {
        _logger.LogInformation("DelayedMessageEventEvent_{event}-----{date}", @event.Message, DateTime.Now);
        return Task.CompletedTask;
    }
}