using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.EventBus.RabbitMQ;

namespace example.net7.api;

public class HoyoEventBusRabbitMQModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddEventBusRabbit(c =>
        {
            c.Host = "117.190.71.69";
            c.Port = 5672;
            c.VirtualHost = "/joe";
            c.UserName = "joe";
            c.PassWord = "dqsf2987";
        });
    }
}
