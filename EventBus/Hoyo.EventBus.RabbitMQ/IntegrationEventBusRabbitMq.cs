﻿using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.EventBus.RabbitMQ.Enums;
using Hoyo.EventBus.RabbitMQ.Extensions;
using Hoyo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Hoyo.EventBus.RabbitMQ;

/// <summary>
/// RabbitMQ集成事件总线
/// </summary>
public class IntegrationEventBusRabbit : IIntegrationEventBus, IDisposable
{
    private readonly IRabbitPersistentConnection _persistentConnection;
    private readonly ILogger<IntegrationEventBusRabbit> _logger;
    private readonly int _retryCount;
    private readonly ISubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private const string HandleName = nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync);
    private bool _isDisposed;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="persistentConnection"></param>
    /// <param name="logger"></param>
    /// <param name="retryCount"></param>
    /// <param name="subsManager"></param>
    /// <param name="serviceProvider"></param>
    public IntegrationEventBusRabbit(IRabbitPersistentConnection persistentConnection, ILogger<IntegrationEventBusRabbit> logger, int retryCount, ISubscriptionsManager subsManager, IServiceProvider serviceProvider)
    {
        _persistentConnection = persistentConnection;
        _logger = logger;
        _retryCount = retryCount;
        _subsManager = subsManager;
        _serviceProvider = serviceProvider;
        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    /// <summary>
    /// 发布消息
    /// </summary>
    /// <param name="event"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    public void Publish<T>(T @event) where T : IIntegrationEvent
    {
        if (!_persistentConnection.IsConnected) _ = _persistentConnection.TryConnect();
        var type = @event.GetType();
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => _logger.LogWarning(ex, "无法发布事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.EventId, $"{time.TotalSeconds:n1}", ex.Message));
        _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.EventId, type.Name);
        var rabbitAttr = type.GetCustomAttribute<RabbitAttribute>();
        if (rabbitAttr is null) throw new($"{nameof(@event)}未设置<{nameof(RabbitAttribute)}>,无法发布事件");
        if (!rabbitAttr.Enable) return;
        if (string.IsNullOrWhiteSpace(rabbitAttr.Queue)) rabbitAttr.Queue = type.Name;
        var channel = _persistentConnection.CreateModel();
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        var headers = @event.GetHeaderAttributes();
        if (headers.Any())
        {
            properties.Headers = headers;
        }
        var args = @event.GetArgAttributes();
        //创建交换机
        channel.ExchangeDeclare(rabbitAttr.Exchange, rabbitAttr.Type, durable: true, arguments: args);
        //创建队列
        //channel.QueueDeclare(queue: rabbitMQAttribute.Queue, durable: false);
        //if (!string.IsNullOrEmpty(rabbitMqAttribute.RoutingKey) && !string.IsNullOrEmpty(rabbitMqAttribute.Queue))
        //{
        //    //通过RoutingKey将队列绑定交换机
        //    channel.QueueBind(rabbitMqAttribute.Queue, rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey);
        //}
        policy.Execute(() =>
        {
            properties.DeliveryMode = 2;
            _logger.LogTrace("向RabbitMQ发布事件: {EventId}", @event.EventId);
            channel.BasicPublish(rabbitAttr.Exchange, rabbitAttr.RoutingKey, true, properties, JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = true }));
        });
    }

    /// <summary>
    /// 基于rabbitmq_delayed_message_exchange插件实现,使用前请确认已安装好插件,发布延时队列消息,需要RabbitMQ开启延时队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="event"></param>
    /// <param name="ttl">若是未指定ttl以及RabbitMQHeader('x-delay',uint)特性将立即消费</param>
    public void Publish<T>(T @event, uint ttl) where T : IIntegrationEvent
    {
        if (!_persistentConnection.IsConnected) _ = _persistentConnection.TryConnect();
        var type = @event.GetType();
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => _logger.LogWarning(ex, "无法发布事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.EventId, $"{time.TotalSeconds:n1}", ex.Message));
        _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.EventId, type.Name);
        var rabbitAttr = type.GetCustomAttribute<RabbitAttribute>();
        if (rabbitAttr is null) throw new($"{nameof(@event)}未设置<{nameof(RabbitAttribute)}>,无法发布事件");
        if (!rabbitAttr.Enable) return;
        if (string.IsNullOrWhiteSpace(rabbitAttr.Queue)) rabbitAttr.Queue = type.Name;
        if (rabbitAttr.Type != EExchange.Delayed.ToDescription()) throw new($"延时队列的交换机类型必须为{EExchange.Delayed}");
        var channel = _persistentConnection.CreateModel();
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        //延时时间从header赋值
        var headers = @event.GetHeaderAttributes();
        var xDelay = headers.TryGetValue("x-delay", out var delay);
        headers["x-delay"] = xDelay && ttl == 0 && delay is not null ? delay : ttl;
        properties.Headers = headers;
        // x-delayed-type 必须加
        var args = @event.GetArgAttributes();
        var xDelayedType = args.TryGetValue("x-delayed-type", out var delayedType);
        args["x-delayed-type"] = !xDelayedType || delayedType is null ? "direct" : delayedType;
        ////创建延时交换机,type类型为x-delayed-message
        channel.ExchangeDeclare(rabbitAttr.Exchange, rabbitAttr.Type, durable: true, autoDelete: false, arguments: args);
        //创建延时消息队列
        //_ = channel.QueueDeclare(queue: rabbitMqAttribute.Queue, durable: true, exclusive: false, autoDelete: false);
        //创建队列
        //channel.QueueDeclare(queue: rabbitMQAttribute.Queue, durable: false);
        //if (!string.IsNullOrEmpty(rabbitMqAttribute.RoutingKey) && !string.IsNullOrEmpty(rabbitMqAttribute.Queue))
        //{
        //    //通过RoutingKey将队列绑定交换机
        //    channel.QueueBind(rabbitMqAttribute.Queue, rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey);
        //}
        policy.Execute(() =>
        {
            properties.DeliveryMode = 2;
            _logger.LogTrace("向RabbitMQ发布事件: {EventId}", @event.EventId);
            channel.BasicPublish(rabbitAttr.Exchange, rabbitAttr.RoutingKey, true, properties, JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = true }));
        });
    }

    /// <summary>
    /// 检查订阅事件是否存在
    /// </summary>
    /// <param name="eventType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private static void CheckEventType(Type eventType)
    {
        //_ = Utils.NotNull(eventType is not null, nameof(eventType));
        if (!eventType.IsDeriveClassFrom<IIntegrationEvent>())
            throw new ArgumentNullException(nameof(eventType), $"{eventType}没有继承{nameof(IIntegrationEvent)}");
    }

    /// <summary>
    /// 检查订阅者是否存在
    /// </summary>
    /// <param name="handlerType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private static void CheckHandlerType(Type handlerType)
    {
        //_ = Utils.NotNull(handlerType, nameof(handlerType));
        if (!handlerType.IsBaseOn(typeof(IIntegrationEventHandler<>)))
            throw new ArgumentNullException(nameof(handlerType), $"{nameof(handlerType)}未派生自IIntegrationEventHandler<>");
    }

    /// <summary>
    /// 集成事件订阅者处理
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void Subscribe()
    {
        if (!_persistentConnection.IsConnected)
        {
            _ = _persistentConnection.TryConnect();
        }
        var handlerTypes = AssemblyHelper.FindTypes(o => o.IsClass && !o.IsAbstract && o.IsBaseOn(typeof(IIntegrationEventHandler<>)));
        foreach (var handlerType in handlerTypes)
        {
            var implementedType = handlerType.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(o => o.IsBaseOn(typeof(IIntegrationEventHandler<>)));
            var eventType = implementedType?.GetTypeInfo().GenericTypeArguments.FirstOrDefault();
            if (eventType is null) continue;
            CheckEventType(eventType);
            CheckHandlerType(handlerType);
            var rabbitAttr = eventType.GetCustomAttribute<RabbitAttribute>();
            if (rabbitAttr is null) throw new($"{nameof(eventType)}未设置<{nameof(RabbitAttribute)}>,无法发布事件");
            if (!rabbitAttr.Enable) continue;
            _ = Task.Factory.StartNew(() =>
            {
                using var consumerChannel = CreateConsumerChannel(rabbitAttr, eventType);
                var eventName = _subsManager.GetEventKey(eventType);
                DoInternalSubscription(eventName, rabbitAttr, consumerChannel);
                _subsManager.AddSubscription(eventType, handlerType);
                StartBasicConsume(eventType, rabbitAttr, consumerChannel);
            });
        }
    }

    private void DoInternalSubscription(string eventName, RabbitAttribute rabbitMqAttribute, IModel consumerChannel)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
        if (containsKey) return;
        if (!_persistentConnection.IsConnected) _ = _persistentConnection.TryConnect();
        consumerChannel.QueueBind(rabbitMqAttribute.Queue, rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey);
    }

    /// <summary>
    /// 创建通道
    /// </summary>
    /// <param name="rabbitMqAttribute"></param>
    /// <param name="eventType"></param>
    /// <returns></returns>
    private IModel CreateConsumerChannel(RabbitAttribute rabbitMqAttribute, Type eventType)
    {
        _logger.LogTrace("创建RabbitMQ消费者通道");
        var channel = _persistentConnection.CreateModel();
        var args = eventType.GetArgAttributes();
        var success = args.TryGetValue("x-delayed-type", out _);
        if (!success && rabbitMqAttribute.Type == EExchange.Delayed.ToDescription())
        {
            args.Add("x-delayed-type", "direct");//x-delayed-type必须加
        }
        //创建交换机
        channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true, autoDelete: false, arguments: args);
        //创建队列
        _ = channel.QueueDeclare(queue: rabbitMqAttribute.Queue, durable: true, exclusive: false, autoDelete: false);
        channel.CallbackException += (_, ea) =>
        {
            _logger.LogWarning(ea.Exception, "重新创建RabbitMQ消费者通道");
            _subsManager.Clear();
            Subscribe();
        };
        return channel;
    }

    private void StartBasicConsume(Type eventType, RabbitAttribute rabbitMqAttribute, IModel? consumerChannel)
    {
        _logger.LogTrace("启动RabbitMQ基本消费");
        if (consumerChannel is not null)
        {
            var consumer = new AsyncEventingBasicConsumer(consumerChannel);
            consumer.Received += async (_, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                try
                {
                    if (message.ToLowerInvariant().Contains("throw-fake-exception")) throw new InvalidOperationException($"假异常请求: \"{message}\"");
                    await ProcessEvent(eventType, message);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "错误处理消息 \"{Message}\"", message);
                }
                // Even on exception we take the message off the queue.
                // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
                // For more information see: https://www.rabbitmq.com/dlx.html
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            };
            _ = consumerChannel.BasicConsume(rabbitMqAttribute.Queue, false, consumer);
            while (true)
            {
                Thread.Sleep(100000);
            }
        }
        _logger.LogError("当_consumerChannel为null时StartBasicConsume不能调用");
    }

    private async Task ProcessEvent(Type eventType, string message)
    {
        var eventName = _subsManager.GetEventKey(eventType);
        _logger.LogTrace("处理RabbitMQ事件: {EventName}", eventName);
        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            using var scope = _serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
            var subscriptionTypes = _subsManager.GetHandlersForEvent(eventName);
            foreach (var subscriptionType in subscriptionTypes)
            {
                var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                if (integrationEvent is null) throw new("集成事件不能为空");
                var method = concreteType.GetMethod(HandleName);
                if (method is null)
                {
                    _logger.LogError("无法找到IIntegrationEventHandler事件处理器下处理者方法");
                    throw new("无法找到IIntegrationEventHandler事件处理器下处理者方法");
                }
                var handler = scope?.ServiceProvider.GetService(subscriptionType);
                if (handler is null) continue;
                await Task.Yield();
                var obj = method.Invoke(handler, new[] { integrationEvent });
                if (obj is null) continue;
                await (Task)obj;
            }
        }
        else _logger.LogWarning("没有订阅RabbitMQ事件: {EventName}", eventName);
    }

    private void SubsManager_OnEventRemoved(object? sender, EventRemovedEventArgs args)
    {
        var eventName = args.EventType?.Name;
        if (!_persistentConnection.IsConnected)
        {
            _ = _persistentConnection.TryConnect();
        }
        using var channel = _persistentConnection.CreateModel();
        var type = args.EventType?.GetCustomAttribute<RabbitAttribute>();
        if (type is null) throw new($"事件未配置[{nameof(RabbitAttribute)}]");
        channel.QueueUnbind(type.Queue ?? eventName, type.Exchange, type.RoutingKey);
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    public void Dispose()
    {
        if (!_isDisposed)
        {
            _subsManager.Clear();
            _isDisposed = true;
        }
        //告诉GC，不要调用析构函数
        GC.SuppressFinalize(this);
    }
}