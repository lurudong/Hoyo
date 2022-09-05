using Hoyo.EventBus;
using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.EventBus.RabbitMQ.Enums;

namespace CustomEvent;
[RabbitMQ("hoyo.test", EExchange.Publish, "", "orderqueue3")]
//[RabbitMQ("hoyo.test", EExchange.Publish, "", "orderqueue2")]
public class CreateOrderEvent : IntegrationEvent
{
    public string Message { get; set; } = default!;
}