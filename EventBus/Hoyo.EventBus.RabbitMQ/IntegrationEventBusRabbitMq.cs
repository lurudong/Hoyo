using Hoyo.EventBus.RabbitMQ.Attributes;
using Hoyo.Extension;
using Hoyo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Hoyo.EventBus.RabbitMQ;

public class IntegrationEventBusRabbitMQ : IIntegrationEventBus, IDisposable
{
    private readonly IRabbitMQPersistentConnection _persistentConnection;
    private readonly ILogger<IntegrationEventBusRabbitMQ> _logger;
    private readonly int _retryCount;
    private readonly IIntegrationEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _handleName = nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync);
    private bool _isDisposed = false;
    public IntegrationEventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, ILogger<IntegrationEventBusRabbitMQ> logger, int retryCount, IIntegrationEventBusSubscriptionsManager subsManager, IServiceProvider serviceProvider)
    {
        _persistentConnection = persistentConnection;
        _logger = logger;
        _retryCount = retryCount;
        _subsManager = subsManager ?? new RabbitMQEventBusSubscriptionsManager();
        _serviceProvider = serviceProvider;
        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    /// <summary>
    /// 发布消息
    /// </summary>
    /// <param name="event"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    public void Publish<TEvent>(IIntegrationEvent @event) where TEvent : IIntegrationEvent
    {
        if (!_persistentConnection.IsConnected) _ = _persistentConnection.TryConnect();
        var type = @event.GetType();
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => _logger.LogWarning(ex, "无法发布事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.EventId, $"{time.TotalSeconds:n1}", ex.Message));
        _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.EventId, type.Name);
        var rabbitMqAttribute = type.GetCustomAttribute<RabbitMQAttribute>();
        if (rabbitMqAttribute is null) throw new($"{nameof(@event)}未设置<RabbitMQAttribute>,无法发布事件");
        if (string.IsNullOrEmpty(rabbitMqAttribute.Queue)) rabbitMqAttribute.Queue = type.Name;
        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });
        using var channel = _persistentConnection.CreateModel();
        var properties = channel.CreateBasicProperties();


        if (rabbitMqAttribute?.Type == Attributes.ExchangeType.DelayedMessage.ToDescription())
        {
            properties.Persistent = true;
        }



        var headers = GetHeaderAttributeFormDic(rabbitMqAttribute, @event);
        if (headers.Any())
        {
            properties.Headers = headers;
        }

        var args = GetArgAttributeFormDic(rabbitMqAttribute, @event);


        //创建交换机
        channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true, arguments: args);
        //创建队列
        //channel.QueueDeclare(queue: rabbitMQAttribute.Queue, durable: false);
        if (!string.IsNullOrEmpty(rabbitMqAttribute.RoutingKey) && !string.IsNullOrEmpty(rabbitMqAttribute.Queue))
        {
            //通过RoutingKey将队列绑定交换机
            channel.QueueBind(rabbitMqAttribute.Queue, rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey);
        }
        policy.Execute(() =>
        {
            properties.DeliveryMode = 2;
            _logger.LogTrace("向RabbitMQ发布事件: {EventId}", @event.EventId);
            channel.BasicPublish(rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey, true, properties, body);
        });
    }

    private IDictionary<string, object> GetHeaderAttributeFormDic(RabbitMQAttribute rabbitMqAttribute, IIntegrationEvent @event)
    {
        var type = @event.GetType();
        var rabbitMQHeaderAttributes = type.GetCustomAttributes<RabbitMQHeaderAttribute>();

        if (rabbitMqAttribute?.Type == Attributes.ExchangeType.DelayedMessage.ToDescription() && !rabbitMQHeaderAttributes.Any())
        {

            throw new($"{nameof(@event)}延时队列未设置<RabbitMQHeaderAttribute>,无法发布事件");
        }


        return this.RabbitDictionarysByDic(rabbitMQHeaderAttributes);

    }


    private IDictionary<string, object> GetArgAttributeFormDic(RabbitMQAttribute rabbitMqAttribute, IIntegrationEvent @event)
    {
        var type = @event.GetType();
        var rabbitMQArgAttributes = type.GetCustomAttributes<RabbitMQArgAttribute>();

        if (rabbitMqAttribute?.Type == Attributes.ExchangeType.DelayedMessage.ToDescription() && !rabbitMQArgAttributes.Any())
        {

            throw new($"{nameof(@event)}延时队列未设置<RabbitMQArgAttribute>,无法发布事件");
        }
        return this.RabbitDictionarysByDic(rabbitMQArgAttributes);

    }

    private IDictionary<string, object> GetArgAttributeFormDic(Type eventType)
    {
        var type = eventType;
        var rabbitMQArgAttributes = type.GetCustomAttributes<RabbitMQArgAttribute>();
        return this.RabbitDictionarysByDic(rabbitMQArgAttributes);

    }
    private IDictionary<string, object> RabbitDictionarysByDic(IEnumerable<RabbitDictionaryAttribute> rabbitDictionaryAttributes)
    {
        IDictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        foreach (var rabbitDictionaryAttribute in rabbitDictionaryAttributes)
        {

            keyValuePairs.Add(rabbitDictionaryAttribute.Key, rabbitDictionaryAttribute.Value);
        }
        return keyValuePairs;
    }
    /// <summary>
    /// 基于rabbitmq_delayed_message_exchange插件实现,使用前请确认已安装好插件,发布延时队列消息,需要RabbitMQ开启延时队列
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <param name="ttl"></param>
    public void PublishWithTTL<TEvent>(IIntegrationEvent @event, uint ttl) where TEvent : IIntegrationEvent
    {
        if (!_persistentConnection.IsConnected) _ = _persistentConnection.TryConnect();
        var type = @event.GetType();
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => _logger.LogWarning(ex, "无法发布事件: {EventId} 超时 {Timeout}s ({ExceptionMessage})", @event.EventId, $"{time.TotalSeconds:n1}", ex.Message));
        _logger.LogTrace("创建RabbitMQ通道来发布事件: {EventId} ({EventName})", @event.EventId, type.Name);
        var rabbitMqAttribute = type.GetCustomAttribute<RabbitMQAttribute>();
        if (rabbitMqAttribute is null) throw new($"{nameof(@event)}未设置<RabbitMQAttribute>,无法发布事件");
        if (string.IsNullOrEmpty(rabbitMqAttribute.Queue)) rabbitMqAttribute.Queue = type.Name;

        if (rabbitMqAttribute.Type != Attributes.ExchangeType.DelayedMessage.ToDescription()) throw new($"延时队列的交换机类型必须为{Attributes.ExchangeType.DelayedMessage}");
        using var channel = _persistentConnection.CreateModel();
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        //延时时间从header赋值
        properties.Headers = new Dictionary<string, object>()
        {
            { "x-delay", ttl }
        };
        Dictionary<string, object> args = new()
        {
            { "x-delayed-type", "direct" } //x-delayed-type必须加
        };
        ////创建延时交换机,type类型为x-delayed-message
        channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true, autoDelete: false, arguments: args);
        //创建延时消息队列
        _ = channel.QueueDeclare(queue: rabbitMqAttribute.Queue, durable: true, exclusive: false, autoDelete: false);
        //创建队列
        //channel.QueueDeclare(queue: rabbitMQAttribute.Queue, durable: false);
        if (!string.IsNullOrEmpty(rabbitMqAttribute.RoutingKey) && !string.IsNullOrEmpty(rabbitMqAttribute.Queue))
        {
            //通过RoutingKey将队列绑定交换机
            channel.QueueBind(rabbitMqAttribute.Queue, rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey);
        }
        policy.Execute(() =>
        {
            properties.DeliveryMode = 2;
            _logger.LogTrace("向RabbitMQ发布事件: {EventId}", @event.EventId);
            channel.BasicPublish(rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey, true, properties, JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        });
    }

    /// <summary>
    /// 对外暴露的订阅者方法
    /// </summary>
    /// <typeparam name="T">传入参数</typeparam>
    /// <typeparam name="TH">处理器</typeparam>
    public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        Subscribe(typeof(T), typeof(TH));
    }

    /// <summary>
    /// 检查订阅事件是否存在
    /// </summary>
    /// <param name="eventType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private static void CheckEventType(Type eventType)
    {
        _ = Utils.NotNull(eventType is not null, nameof(eventType));
        if (eventType?.IsDeriveClassFrom<IIntegrationEvent>() == false)
            throw new ArgumentNullException(nameof(eventType), $"{eventType}没有继承IIntegrationEvent");
    }

    /// <summary>
    /// 检查订阅者是否存在
    /// </summary>
    /// <param name="handlerType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private static void CheckHandlerType(Type handlerType)
    {
        _ = Utils.NotNull(handlerType, nameof(handlerType));
        if (handlerType.IsBaseOn(typeof(IIntegrationEventHandler<>)) == false)
            throw new ArgumentNullException(nameof(handlerType), $"{nameof(handlerType)}IIntegrationEventHandler<>");
    }

    /// <summary>
    /// 集成事件订阅者处理
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="handlerType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Subscribe(Type eventType, Type handlerType)
    {

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
            var implementedType = handlerType.GetTypeInfo().ImplementedInterfaces.Where(o => o.IsBaseOn(typeof(IIntegrationEventHandler<>))).FirstOrDefault();
            var eventType = implementedType?.GetTypeInfo().GenericTypeArguments.FirstOrDefault();
            if (eventType == null)
            {
                continue;
            }
            CheckEventType(eventType);
            CheckHandlerType(handlerType);
            var rabbitMqAttribute = eventType.GetCustomAttribute<RabbitMQAttribute>();
            if (rabbitMqAttribute == null) throw new($"{nameof(eventType)}未设置<RabbitMQAttribute>,无法发布事件");
            _ = Task.Factory.StartNew(() =>
            {
                using var consumerChannel = CreateConsumerChannel(rabbitMqAttribute, eventType);
                var eventName = _subsManager.GetEventKey(eventType);
                DoInternalSubscription(eventName, rabbitMqAttribute, consumerChannel);
                _subsManager.AddSubscription(eventType, handlerType);
                StartBasicConsume(eventType, rabbitMqAttribute, consumerChannel);
            });
        }
    }

    /// <summary>
    /// 创建通道
    /// </summary>
    /// <param name="rabbitMqAttribute"></param>
    /// <returns></returns>
    private IModel CreateConsumerChannel(RabbitMQAttribute rabbitMqAttribute, Type eventType)
    {
        _logger.LogTrace("创建RabbitMQ消费者通道");
        var channel = _persistentConnection.CreateModel();

        //var args = rabbitMqAttribute.Type == Attributes.ExchangeType.DelayedMessage.ToDescription()
        //    ? new Dictionary<string, object>()
        //    {
        //        { "x-delayed-type", "direct" } //x-delayed-type必须加
        //    } : null;

        var args = GetArgAttributeFormDic(eventType);
        //创建交换机
        channel.ExchangeDeclare(rabbitMqAttribute.Exchange, rabbitMqAttribute.Type, durable: true, autoDelete: false, arguments: args.Any() ? args : null);
        //创建队列
        _ = channel.QueueDeclare(queue: rabbitMqAttribute.Queue, durable: true, exclusive: false, autoDelete: false);
        channel.CallbackException += (sender, ea) =>
        {
            _logger.LogWarning(ea.Exception, "重新创建RabbitMQ消费者通道");
            _subsManager.Clear();
            Subscribe();
        };
        return channel;
    }

    private void StartBasicConsume(Type eventType, RabbitMQAttribute rabbitMqAttribute, IModel? consumerChannel) //where T : IntegrationEvent
    {
        _logger.LogTrace("启动RabbitMQ基本消费");
        if (consumerChannel is not null)
        {
            var consumer = new AsyncEventingBasicConsumer(consumerChannel);
            //consumer.Received += Consumer_Received;
            consumer.Received += async (model, ea) =>
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
        else _logger.LogError("当_consumerChannel为null时StartBasicConsume不能调用");
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
                //var eventType = _subsManager.GetEventTypeByName(eventName);
                var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                if (integrationEvent is null) throw new("集成事件不能为空。。。");
                var method = concreteType.GetMethod(_handleName);
                if (method == null)
                {
                    _logger.LogError("无法找到IIntegrationEventHandler事件处理器,下处理者方法");
                    throw new("无法找到IIntegrationEventHandler事件处理器,下处理者方法");
                }
                var handler = scope?.ServiceProvider.GetService(subscriptionType);
                if (handler == null) continue;
                await Task.Yield();
                var obj = method.Invoke(handler, new object[] { integrationEvent });
                if (obj == null) continue;
                await (Task)obj;
            }
        }
        else _logger.LogWarning("没有订阅RabbitMQ事件: {EventName}", eventName);
    }

    private void DoInternalSubscription(string eventName, RabbitMQAttribute rabbitMqAttribute, IModel consumerChannel)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
        if (!containsKey)
        {
            if (!_persistentConnection.IsConnected) _ = _persistentConnection.TryConnect();
            consumerChannel.QueueBind(rabbitMqAttribute.Queue, rabbitMqAttribute.Exchange, rabbitMqAttribute.RoutingKey);
        }
    }

    public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();
        _logger.LogInformation("移除事件 {EventName}", eventName);
        _subsManager.RemoveSubscription<T, TH>();
    }

    private void SubsManager_OnEventRemoved(object? sender, EventRemovedEventArgs args)
    {
        var eventName = args.EventType?.Name;
        if (!_persistentConnection.IsConnected)
        {
            _ = _persistentConnection.TryConnect();
        }
        using var channel = _persistentConnection.CreateModel();
        var type = args.EventType?.GetCustomAttribute<RabbitMQAttribute>();
        if (type is null) throw new($"事件未配置[RabbitMQAttribute]");
        channel.QueueUnbind(type.Queue ?? eventName, type.Exchange, type.RoutingKey);
    }

    /// <summary>
    /// 释放对象和链接
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _subsManager.Clear();
            }
            _isDisposed = true;
        }
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        //告诉GC，不要调用析构函数
        GC.SuppressFinalize(this);
    }
}