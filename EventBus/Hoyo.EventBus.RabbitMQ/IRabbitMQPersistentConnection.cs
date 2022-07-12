using RabbitMQ.Client;

namespace Hoyo.EventBus.RabbitMQ;

public interface IRabbitMQPersistentConnection : IDisposable
{
    bool IsConnected { get;  }

    bool TryConnect();

    IModel CreateModel();
}