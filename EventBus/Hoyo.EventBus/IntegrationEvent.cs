using System.Text.Json.Serialization;

namespace Hoyo.EventBus;

public class IntegrationEvent : IIntegrationEvent
{
    public IntegrationEvent()
    {
        EventId = Guid.NewGuid().ToString();
        EventCreationDate = DateTime.Now;
    }
    [JsonInclude]
    public string EventId { get; }
    [JsonInclude]
    public DateTime EventCreationDate { get; }
}