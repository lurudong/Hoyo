namespace Hoyo.EventBus.RabbitMQ.Attributes;

/// <summary>
/// 添加RabbitMQ,Headers参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RabbitHeaderAttribute : RabbitDictionaryAttribute
{
    public RabbitHeaderAttribute(string key, object value) : base(key, value) { }
}