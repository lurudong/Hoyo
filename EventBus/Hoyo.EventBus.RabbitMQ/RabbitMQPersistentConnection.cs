using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace Hoyo.EventBus.RabbitMQ;

public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQPersistentConnection> _logger;
    private readonly int _retryCount;
    private IConnection? _connection;
    private bool _disposed;
    private readonly object sync_root = new();

    public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQPersistentConnection> logger, int retryCount = 5)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _retryCount = retryCount;
    }

    public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    public IModel CreateModel() => !IsConnected
            ? throw new InvalidOperationException("RabbitMQ连接失败")
            : _connection is null ? throw new InvalidOperationException("RabbitMQ连接未创建") : _connection.CreateModel();

    public bool TryConnect()
    {
        _logger.LogInformation("RabbitMQ客户端尝试连接");
        lock (sync_root)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => _logger.LogWarning(ex, "RabbitMQ客户端在{TimeOut}s超时后无法创建链接,({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message));
            _ = policy.Execute(() => _connection = _connectionFactory.CreateConnection());
            if (IsConnected && _connection is not null)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;
                _logger.LogInformation("RabbitMQ客户端获取了与[{HostName}]的持久连接,并订阅了故障事件", _connection.Endpoint.HostName);
                _disposed = false;
                return true;
            }
            else
            {
                _logger.LogCritical("RabbitMQ连接不能被创建和打开");
                return false;
            }
        }
    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;
        _logger.LogWarning("RabbitMQ连接关闭,正在尝试重新连接...");
        _ = TryConnect();
    }

    void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;
        _logger.LogWarning("RabbitMQ连接抛出异常,在重试...");
        _ = TryConnect();
    }

    void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
    {
        if (_disposed) return;
        _logger.LogWarning("RabbitMQ连接处于关闭状态,正在尝试重新连接...");
        _ = TryConnect();
    }
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        if (_connection is not null)
        {
            try
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(message: "{message}", ex.Message);
            }
        }
        GC.SuppressFinalize(this);
    }
}