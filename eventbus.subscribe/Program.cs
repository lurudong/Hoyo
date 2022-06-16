using eventbus.events;
using eventbus.subscribe.IntegrationEvents.EventHandling;
using Hoyo.EventBus;
using Hoyo.EventBus.Abstractions;
using Hoyo.EventBus.RabbitMQ;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

_ = builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBusConnection"],
        DispatchConsumersAsync = true,
        Port = int.Parse(builder.Configuration["EventBusPort"]!)
    };

    if (!string.IsNullOrEmpty(builder.Configuration["EventBusUserName"]))
    {
        factory.UserName = builder.Configuration["EventBusUserName"];
    }

    if (!string.IsNullOrEmpty(builder.Configuration["EventBusPassword"]))
    {
        factory.Password = builder.Configuration["EventBusPassword"];
    }

    var retryCount = 5;
    if (!string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
    {
        retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]!);
    }

    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
});

_ = builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
{
    var subscriptionClientName = builder.Configuration["SubscriptionClientName"];
    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
    var retryCount = 5;
    if (!string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
    {
        retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]!);
    }

    return new EventBusRabbitMQ(sp, rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
});
_ = builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

//注册要订阅事件的处理器
//services.AddTransient<ProductPriceChangedIntegrationEventHandler>();
_ = builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

var eventBus = app.Services.GetRequiredService<IEventBus>();
//启用订阅事件
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
eventBus.Subscribe<TestSecondEvent, TestSecondEventHandler>();

app.Run();
