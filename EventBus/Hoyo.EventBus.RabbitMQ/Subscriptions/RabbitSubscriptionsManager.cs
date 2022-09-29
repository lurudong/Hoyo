using System.Collections.Concurrent;

namespace Hoyo.EventBus.RabbitMQ;
/// <summary>
/// RabbitMQ订阅管理器
/// </summary>
public class RabbitSubscriptionsManager : ISubscriptionsManager
{
    private readonly ConcurrentDictionary<string, List<Type>> _handlers = new();
    /// <summary>
    /// 添加订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="THandler"></typeparam>
    public void AddSubscription<T, THandler>() where T : IntegrationEvent where THandler : IIntegrationEventHandler<T>
    {
        var eventKey = GetEventKey<T>();
        DoAddSubscription(typeof(THandler), eventKey);
    }
    /// <summary>
    /// 添加订阅
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="handlerType"></param>
    public void AddSubscription(Type eventType, Type handlerType)
    {
        var eventKey = GetEventKey(eventType);
        DoAddSubscription(handlerType, eventKey);
    }

    private void DoAddSubscription(Type handlerType, string eventName)
    {
        if (!HasSubscriptionsForEvent(eventName)) _ = _handlers.TryAdd(eventName, new());
        if (_handlers[eventName].Any(o => o == handlerType)) throw new ArgumentException($"类型:{handlerType.Name} 已注册 '{eventName}'", nameof(handlerType));
        _handlers[eventName].Add(handlerType);
    }
    /// <summary>
    /// 从事件获取事件处理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<Type> GetHandlersForEvent<T>() where T : IIntegrationEvent
    {
        var eventKey = GetEventKey<T>();
        return GetHandlersForEvent(eventKey);
    }
    /// <summary>
    /// 从事件获取事件处理器
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public IEnumerable<Type> GetHandlersForEvent(string eventName) => _handlers[eventName];
    /// <summary>
    /// 是否有事件订阅
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);
    /// <summary>
    /// 是否有事件订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
    {
        var eventKey = GetEventKey<T>();
        return HasSubscriptionsForEvent(eventKey);
    }
    /// <summary>
    /// 移除订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="THandler"></typeparam>
    public void RemoveSubscription<T, THandler>() where T : IntegrationEvent where THandler : IIntegrationEventHandler<T>
    {
        if (!HasSubscriptionsForEvent<T>()) return;
        var eventName = GetEventKey<T>();
        _ = _handlers[eventName].Remove(typeof(THandler));
        if (_handlers[eventName].Any()) return;
        //_handlers.Remove(eventName);
        EventRemovedEventArgs args = new()
        {
            EventType = typeof(T)
        };
        OnEventRemoved?.Invoke(this, args);
    }
    /// <summary>
    /// 当事件移除
    /// </summary>
    public event EventHandler<EventRemovedEventArgs>? OnEventRemoved;
    /// <summary>
    /// 是否为空
    /// </summary>
    public bool IsEmpty => !_handlers.Keys.Any();
    /// <summary>
    /// 清空
    /// </summary>
    public void Clear() => _handlers.Clear();
    /// <summary>
    /// 获取事件Key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public string GetEventKey<T>() => GetEventKey(typeof(T));
    /// <summary>
    /// 获取事件Key
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetEventKey(Type type) => type.Name;
}