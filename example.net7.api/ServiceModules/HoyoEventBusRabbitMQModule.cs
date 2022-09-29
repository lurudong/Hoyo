using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.EventBus.RabbitMQ;

namespace example.net7.api;

/// <summary>
/// 消息总线模块
/// </summary>
public class HoyoEventBusRabbitMQModule : AppModule
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public HoyoEventBusRabbitMQModule()
    {
        Enable = false;
    }
    /// <summary>
    /// 配置和注册服务
    /// </summary>
    /// <param name="context"></param>
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
