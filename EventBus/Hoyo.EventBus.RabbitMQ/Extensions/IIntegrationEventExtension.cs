using Hoyo.EventBus.RabbitMQ.Attributes;
using System.Reflection;

namespace Hoyo.EventBus.RabbitMQ.Extensions;
internal static class IIntegrationEventExtension
{
    internal static IDictionary<string, object> GetHeaderAttributes(this IIntegrationEvent @event)
    {
        var type = @event.GetType();
        var rabbitMQHeaderAttributes = type.GetCustomAttributes<RabbitMQHeaderAttribute>();
        return RabbitDictionarysByDic(rabbitMQHeaderAttributes);
    }

    internal static IDictionary<string, object> GetArgAttributes(this IIntegrationEvent @event)
    {
        var type = @event.GetType();
        var rabbitMQArgAttributes = type.GetCustomAttributes<RabbitMQArgAttribute>();
        return RabbitDictionarysByDic(rabbitMQArgAttributes);
    }

    internal static IDictionary<string, object> GetArgAttributes(this Type eventType)
    {
        var rabbitMQArgAttributes = eventType.GetCustomAttributes<RabbitMQArgAttribute>();
        return RabbitDictionarysByDic(rabbitMQArgAttributes);
    }

    internal static IDictionary<string, object> RabbitDictionarysByDic(this IEnumerable<RabbitDictionaryAttribute> rabbitDictionaryAttributes)
    {
        var keyValuePairs = new Dictionary<string, object>();
        foreach (var rabbitDictionaryAttribute in rabbitDictionaryAttributes)
        {
            keyValuePairs.Add(rabbitDictionaryAttribute.Key, rabbitDictionaryAttribute.Value);
        }
        return keyValuePairs;
    }
}