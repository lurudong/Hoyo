using RabbitMQ.Client;

namespace Hoyo.EventBus.RabbitMQ;
/// <summary>
/// RabbitMQ持久化链接接口
/// </summary>
public interface IRabbitPersistentConnection : IDisposable
{
    /// <summary>
    /// 是否链接
    /// </summary>
    bool IsConnected { get; }
    /// <summary>
    /// 尝试链接
    /// </summary>
    /// <returns></returns>
    bool TryConnect();
    /// <summary>
    /// 创建Model
    /// </summary>
    /// <returns></returns>
    IModel CreateModel();
}