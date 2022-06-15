using Hoyo.EventBus.Events;

namespace Hoyo.EventBus.Abstractions;

/// <summary>
/// 集成事件处理器接口
/// </summary>
/// <typeparam name="TIntegrationEvent">TIntegrationEvent泛型</typeparam>
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler   where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}

/// <summary>
/// 集成事件处理器
/// </summary>
public interface IIntegrationEventHandler
{
}
