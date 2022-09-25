using Hoyo.EventBus.RabbitMQ.Enums;
using Hoyo.Extensions;

namespace Hoyo.EventBus.RabbitMQ.Attributes;

/// <summary>
/// 应用交换机队列等参数
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RabbitAttribute : Attribute
{
    public RabbitAttribute(string exchange, EExchange exchangeType, string routingKey, string? queue = null, bool enable = true)
    {
        Exchange = exchange;
        Type = exchangeType.ToDescription() ?? "direct";
        RoutingKey = routingKey;
        Queue = queue;
        Enable = enable;
    }
    /// <summary>
    /// 交换机
    /// </summary>
    public string Exchange { get; set; }
    /// <summary>
    /// 交换机模式
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 路由键《路由键和队列名称配合使用》
    /// </summary>
    public string RoutingKey { get; set; }
    /// <summary>
    /// 队列名称《队列名称和路由键配合使用》
    /// </summary>
    public string? Queue { get; set; }
    /// <summary>
    /// 是否启用队列
    /// </summary>
    public bool Enable { get; set; }
}