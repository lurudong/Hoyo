namespace Hoyo.EventBus;

/// <summary>
/// 发送事件接口定义
/// </summary>
public interface IIntegrationEventBus
{
    /// <summary>
    /// 发送事件
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    /// <param name="event">事件对象</param>
    void Publish<T>(T @event) where T : IIntegrationEvent;
    /// <summary>
    /// 延时事件发送
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    /// <param name="event">事件对象</param>
    /// <param name="ttl">延时时长</param>
    void Publish<T>(T @event, uint ttl) where T : IIntegrationEvent;
    /// <summary>
    /// 事件订阅
    /// </summary>
    void Subscribe();
    //void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    //void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
}
