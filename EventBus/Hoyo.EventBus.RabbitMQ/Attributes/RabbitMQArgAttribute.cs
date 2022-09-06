namespace Hoyo.EventBus.RabbitMQ.Attributes;

/// <summary>
/// 参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RabbitMQArgAttribute : RabbitDictionaryAttribute
{
    public RabbitMQArgAttribute(string key, object value) : base(key, value) { }
}
