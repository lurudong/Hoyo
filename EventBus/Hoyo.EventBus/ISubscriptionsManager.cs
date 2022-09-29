namespace Hoyo.EventBus;

/// <summary>
/// 订阅管理器接口
/// </summary>
public interface ISubscriptionsManager
{
    /// <summary>
    /// 事件移除事件处理器
    /// </summary>
    event EventHandler<EventRemovedEventArgs> OnEventRemoved;
    /// <summary>
    /// 是否为空
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// 清除所有订阅
    /// </summary>
    void Clear();

    /// <summary>
    /// 添加订阅
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    /// <typeparam name="TH">事件处理器类型</typeparam>
    void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    /// <summary>
    /// 添加订阅
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handlerType">事件处理器类型</param>
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
    /// <summary>
    /// 获取事件Key
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    string GetEventKey(Type type);
}
/// <summary>
/// 事件移除参数
/// </summary>
public class EventRemovedEventArgs : EventArgs
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public Type? EventType { get; set; }
}