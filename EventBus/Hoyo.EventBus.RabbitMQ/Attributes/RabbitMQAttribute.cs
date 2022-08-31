using Hoyo.Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hoyo.EventBus.RabbitMQ.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RabbitMQAttribute : Attribute
{

    public RabbitMQAttribute(string exchange, ExchangeType exchangeType, string routingKey, string? queue = null)
    {
        Exchange = exchange;
        Type = exchangeType.ToDescription() ?? "direct";
        RoutingKey = routingKey;
        Queue = queue;
        //Args = args ?? new Dictionary<string, object>();
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


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
/// <summary>
/// 头参数特性
/// </summary>
public class RabbitMQHeaderAttribute : RabbitDictionaryAttribute

{


    public RabbitMQHeaderAttribute(string key, object value) : base(key, value)
    {


    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
/// <summary>
/// 参数特性
/// </summary>
public class RabbitMQArgAttribute : RabbitDictionaryAttribute

{


    public RabbitMQArgAttribute(string key, object value) : base(key, value)
    {


    }
}
public class RabbitDictionaryAttribute : Attribute
{
    public RabbitDictionaryAttribute(string key, object value)
    {

        Key = key;
        Value = value;
    }

    public string Key { get; }

    public object Value { get; }

}

public enum ExchangeType
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