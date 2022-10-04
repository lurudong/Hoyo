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
        Enable = !false;
    }
    /// <summary>
    /// 配置和注册服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddEventBusRabbit(c =>
        {
            c.Host = "101.34.26.221";
            c.Port = 40003;
            c.UserName = "admin";
            c.PassWord = "&oneblogs789";
        });
    }
}
