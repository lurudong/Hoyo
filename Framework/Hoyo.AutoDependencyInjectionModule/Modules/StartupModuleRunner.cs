using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;
/// <summary>
/// 启动模块运行器
/// </summary>
public class StartupModuleRunner : ModuleApplicationBase, IStartupModuleRunner
{
    /// <summary>
    /// 程序启动运行时
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="services"></param>
    public StartupModuleRunner(Type startupModuleType, IServiceCollection services) : base(startupModuleType, services) => services.AddSingleton<IStartupModuleRunner>(this);
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        IocManage.Instance.SetServiceCollection(services);
        var context = new ConfigureServicesContext(services);
        _ = services.AddSingleton(context);
        foreach (var module in Modules)
        {
            //如果是继承了AppModule
            if (module is AppModule appModule)
            {
                appModule.ConfigureServicesContext = context;
            }
        }
        foreach (var config in Modules)
        {
            _ = services.AddSingleton(config);
            config.ConfigureServices(context);
        }
        foreach (var module in Modules)
        {
            if (module is AppModule appModule)
            {
                appModule.ConfigureServicesContext = null!;
            }
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="service"></param>
    public void Initialize(IServiceProvider service)
    {
        IocManage.Instance.SetApplicationServiceProvider(service);
        SetServiceProvider(service);
        using var scope = ServiceProvider?.CreateScope();
        //using var scope = service.CreateScope();
        var ctx = new ApplicationContext(scope!.ServiceProvider);
        foreach (var cfg in Modules)
        {
            cfg.ApplicationInitialization(ctx);
        }
    }
    /// <summary>
    /// Dispose
    /// </summary>
    public new void Dispose()
    {
        base.Dispose();
        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}