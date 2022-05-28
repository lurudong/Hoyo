using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.AutoDependencyInjectionModule.Modules;

namespace example.api;

/**
 * 要实现自动注入,一定要在这个地方添加
 */
[DependsOn(
    typeof(DependencyAppModule),
    typeof(CorsModule),
    typeof(HoyoMongoModule),
    typeof(SwaggerModule),
    typeof(MyTestModule)
)]
public class AppWebModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        base.ConfigureServices(context);
    }
}