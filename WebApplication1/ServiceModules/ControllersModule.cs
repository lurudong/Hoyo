using Hoyo.AutoDependencyInjectionModule.Modules;

namespace WebApplication1;

public class ControllersModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        _ = context.Services.AddControllers();
        _ = context.Services.AddEndpointsApiExplorer();
    }
}
