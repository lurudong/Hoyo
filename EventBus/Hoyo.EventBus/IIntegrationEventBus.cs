namespace Hoyo.EventBus;

/// <summary>
/// 发送事件接口定义
/// </summary>
public interface IIntegrationEventBus
{
    void Publish<T>(T @event) where T : IIntegrationEvent;
    void Publish<T>(T @event, uint ttl) where T : IIntegrationEvent;
    void Subscribe();
    //void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    //void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
}
