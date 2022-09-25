namespace Hoyo.EventBus.RabbitMQ.Attributes;

/// <summary>
/// RabbitMQ参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RabbitArgAttribute : RabbitDictionaryAttribute
{
    public RabbitArgAttribute(string key, object value) : base(key, value) { }
}
