using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.EventBus.RabbitMQ;

namespace example.net7.api;

public class HoyoEventBusRabbitMQModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddEventBusRabbitMQ(x =>
        {
            x.UserName = "admin";
            x.Host = "222.83.110.103";
            x.Port = 40004;
            x.PassWord = "&oneblogs789";
        });
    }
}
