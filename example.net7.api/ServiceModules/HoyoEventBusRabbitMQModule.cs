using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.EventBus.RabbitMQ;

namespace example.net7.api;

public class HoyoEventBusRabbitMQModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        _ = context.Services.AddEventBusRabbitMq(x =>
        {
            x.UserName = "admin";
            x.Host = "101.34.26.221";
            x.Port = 40013;
            x.PassWord = "&duyu789";
        });
    }
}
