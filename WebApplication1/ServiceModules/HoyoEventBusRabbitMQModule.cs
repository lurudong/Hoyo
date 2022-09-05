using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.EventBus.RabbitMQ;

namespace WebApplication1;

public class HoyoEventBusRabbitMQModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddEventBusRabbitMQ(x =>
        {
            x.UserName = "admin";
            x.Host = "117.190.71.69";
            x.Port = 5672;
            x.PassWord = "dqsf2987";
        });
    }
}
