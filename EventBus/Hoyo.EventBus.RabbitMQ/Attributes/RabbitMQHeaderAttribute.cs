namespace Hoyo.EventBus.RabbitMQ.Attributes;

/// <summary>
/// 头参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RabbitMQHeaderAttribute : RabbitDictionaryAttribute
{
    public RabbitMQHeaderAttribute(string key, object value) : base(key, value) { }
}