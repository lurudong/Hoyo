using System.ComponentModel;

namespace Hoyo.EventBus.RabbitMQ.Enums;
public enum EExchange
{
    /// <summary>
    /// 路由模式
    /// </summary>
    [Description("direct")]
    Routing = 1,
    /// <summary>
    /// 主题模式
    /// </summary>
    [Description("topic")]
    Topic = 2,
    /// <summary>
    /// 订阅模式
    /// </summary>
    [Description("fanout")]
    Subscribe = 3,
    /// <summary>
    /// 延时x-delayed-message模式
    /// </summary>
    [Description("x-delayed-message")]
    DelayedMessage = 4
}
