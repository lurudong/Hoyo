using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.EventBus.RabbitMQ;

namespace example.net7.api;

public class HoyoEventBusRabbitMQModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddEventBusRabbitMQ(c =>
        {
            c.UserName = "admin";
            c.Host = "101.34.26.221";
            c.Port = 40003;
            c.PassWord = "&oneblogs789";
            c.VirtualHost = "/";
        });
    }
}
