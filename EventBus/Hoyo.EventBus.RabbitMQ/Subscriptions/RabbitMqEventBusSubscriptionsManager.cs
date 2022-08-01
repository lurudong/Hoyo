using System.Collections.Concurrent;

namespace Hoyo.EventBus.RabbitMQ;

public class RabbitMqEventBusSubscriptionsManager : IIntegrationEventBusSubscriptionsManager
{
    private readonly ConcurrentDictionary<string, List<Type>> _handlers = new();

    public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        var eventKey = GetEventKey<T>();
        DoAddSubscription(typeof(TH), eventKey);
    }

    public void AddSubscription(Type eventType, Type handlerType)
    {
        var eventKey = GetEventKey(eventType);
        DoAddSubscription(handlerType, eventKey);
    }

    private void DoAddSubscription(Type handlerType, string eventName)
    {

        if (!HasSubscriptionsForEvent(eventName))
        {
            _ = _handlers.TryAdd(eventName, new List<Type>());
        }
        if (_handlers[eventName].Any(o => o == handlerType))
        {
            throw new ArgumentException($"类型:{handlerType.Name} 已注册 '{eventName}'", nameof(handlerType));
        }
        _handlers[eventName].Add(handlerType);
    }

    public IEnumerable<Type> GetHandlersForEvent<T>() where T : IIntegrationEvent
    {
        var eventKey = GetEventKey<T>();
        return GetHandlersForEvent(eventKey);
    }

    public IEnumerable<Type> GetHandlersForEvent(string eventName) => _handlers[eventName];

    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

    public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
    {
        var eventKey = GetEventKey<T>();
        return HasSubscriptionsForEvent(eventKey);
    }

    public void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        if (!HasSubscriptionsForEvent<T>())
        {
            return;
        }
        var eventName = GetEventKey<T>();
        _ = _handlers[eventName].Remove(typeof(TH));
        if (!_handlers[eventName].Any())
        {
            //_handlers.Remove(eventName);
            EventRemovedEventArgs args = new()
            {
                EventType = typeof(T)
            };
            OnEventRemoved?.Invoke(this, args);
        }
    }

    public event EventHandler<EventRemovedEventArgs>? OnEventRemoved;
    public bool IsEmpty => !_handlers.Keys.Any();
    public void Clear() => _handlers.Clear();
    public string GetEventKey<T>() => GetEventKey(typeof(T));
    public string GetEventKey(Type type) => type.Name;
}
