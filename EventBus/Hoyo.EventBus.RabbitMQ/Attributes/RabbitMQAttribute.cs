using Hoyo.EventBus.RabbitMQ.Enums;
using Hoyo.Extensions;

namespace Hoyo.EventBus.RabbitMQ.Attributes;

/// <summary>
/// 当同一个队列应用多个特性的时候,务必使用Publish交换机模式
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RabbitMQAttribute : Attribute
{
    public RabbitMQAttribute(string exchange, EExchange exchangeType, string routingKey, string? queue = null)
    {
        Exchange = exchange;
        Type = exchangeType.ToDescription() ?? "direct";
        RoutingKey = routingKey;
        Queue = queue;
    }
    /// <summary>
    /// 交换机
    /// </summary>
    public string Exchange { get; set; } = default!;
    /// <summary>
    /// 交换机模式
    /// </summary>
    public string Type { get; set; } = default!;
    /// <summary>
    /// 路由键《路由键和队列名称配合使用》
    /// </summary>
    public string RoutingKey { get; set; } = default!;
    /// <summary>
    /// 队列名称《队列名称和路由键配合使用》
    /// </summary>
    public string? Queue { get; set; }
}