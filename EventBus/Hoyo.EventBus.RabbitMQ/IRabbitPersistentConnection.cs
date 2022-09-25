using RabbitMQ.Client;

namespace Hoyo.EventBus.RabbitMQ;

public interface IRabbitPersistentConnection : IDisposable
{
    bool IsConnected { get; }
    bool TryConnect();
    IModel CreateModel();
}