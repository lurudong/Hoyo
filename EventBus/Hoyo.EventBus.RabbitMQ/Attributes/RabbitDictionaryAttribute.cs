namespace Hoyo.EventBus.RabbitMQ.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
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
