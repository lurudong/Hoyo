using Hoyo.EventBus.RabbitMQ.Attributes;
using System.Reflection;

namespace Hoyo.EventBus.RabbitMQ.Extensions;
internal static class IntegrationEventExtension
{
    internal static IDictionary<string, object> GetHeaderAttributes(this IIntegrationEvent @event)
    {
        var type = @event.GetType();
        var rabbitHeaderAttributes = type.GetCustomAttributes<RabbitHeaderAttribute>();
        return RabbitDictionariesByDic(rabbitHeaderAttributes);
    }

    internal static IDictionary<string, object> GetArgAttributes(this IIntegrationEvent @event)
    {
        var type = @event.GetType();
        var rabbitArgAttributes = type.GetCustomAttributes<RabbitArgAttribute>();
        return RabbitDictionariesByDic(rabbitArgAttributes);
    }

    internal static IDictionary<string, object> GetArgAttributes(this Type eventType)
    {
        var rabbitArgAttributes = eventType.GetCustomAttributes<RabbitArgAttribute>();
        return RabbitDictionariesByDic(rabbitArgAttributes);
    }

    internal static IDictionary<string, object> RabbitDictionariesByDic(this IEnumerable<RabbitDictionaryAttribute> rabbitDictionaryAttributes) => 
        rabbitDictionaryAttributes.ToDictionary(rabbitDictionaryAttribute => rabbitDictionaryAttribute.Key, rabbitDictionaryAttribute => rabbitDictionaryAttribute.Value);
}