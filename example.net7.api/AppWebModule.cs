using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.WebCore;

namespace example.net7.api;

/**
 * 要实现自动注入,一定要在这个地方添加
 */
[DependsOn(
    typeof(DependencyAppModule),
    typeof(CorsModule),
    typeof(ControllersModule),
    typeof(HoyoMongoModule),
    typeof(HoyoMongoGridFSModule),
    typeof(SwaggerModule),
    typeof(HoyoEventBusRabbitMQModule),
    typeof(MyTestModule)
)]
public class AppWebModule : AppModule
{
    /// <summary>
    /// 注册和配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        base.ConfigureServices(context);
        _ = context.Services.AddHttpContextAccessor();
    }
    /// <summary>
    /// 注册中间件
    /// </summary>
    /// <param name="context"></param>
    public override void ApplicationInitialization(ApplicationContext context)
    {
        base.ApplicationInitialization(context);
        var app = context.GetApplicationBuilder();
        _ = app.UseHoyoResponseTime();
        _ = app.UseAuthorization();
    }
}