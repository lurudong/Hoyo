using Hoyo.EventBus.Events;

namespace eventbus.events;

public record TestSecondEvent : IntegrationEvent
{
    public string Name { get; init; }

    public TestSecondEvent(string name) => Name = name;
}