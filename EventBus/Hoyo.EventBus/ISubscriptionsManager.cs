﻿namespace Hoyo.EventBus;

public interface ISubscriptionsManager
{

    event EventHandler<EventRemovedEventArgs> OnEventRemoved;
    bool IsEmpty { get; }
    void Clear();

    /// <summary>
    /// 添加订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

    void AddSubscription(Type eventType, Type handlerType);

    /// <summary>
    /// 移除订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// 获取事件处理程序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>

    IEnumerable<Type> GetHandlersForEvent<T>() where T : IIntegrationEvent;

    /// <summary>
    /// 获取事件处理程序
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    IEnumerable<Type> GetHandlersForEvent(string eventName);

    /// <summary>
    /// 判断订阅者是否存在
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    bool HasSubscriptionsForEvent(string eventName);

    /// <summary>
    /// 判断订阅者是否存在
    /// </summary>
    /// <returns></returns>
    bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

    /// <summary>
    /// 获取事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    string GetEventKey<T>();

    string GetEventKey(Type type);
}

public class EventRemovedEventArgs : EventArgs
{
    public Type? EventType { get; set; }
}