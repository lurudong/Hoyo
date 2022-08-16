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
            x.Host = "222.83.110.112";
            x.PassWord = "dqsf2987";
        });
    }
}
