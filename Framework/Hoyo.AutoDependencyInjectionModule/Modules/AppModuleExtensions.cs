using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;
/// <summary>
/// 应用模块扩展.
/// </summary>
public static class AppModuleExtensions
{
    /// <summary>
    /// 添加应用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplication<T>(this IServiceCollection services) where T : IAppModule
    {
        _ = services.AddApplication(typeof(T));
        return services;
    }
    /// <summary>
    /// 添加应用
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static IServiceCollection AddApplication(this IServiceCollection services, Type type)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        var obj = new ObjectAccessor<IApplicationBuilder>();
        services.Add(ServiceDescriptor.Singleton(typeof(ObjectAccessor<IApplicationBuilder>), obj));
        services.Add(ServiceDescriptor.Singleton(typeof(IObjectAccessor<IApplicationBuilder>), obj));
        IStartupModuleRunner runner = new StartupModuleRunner(type, services);
        runner.ConfigureServices(services);
        return services;
    }
    /// <summary>
    /// 初始化应用
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder InitializeApplication(this IApplicationBuilder builder)
    {
        builder.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = builder;
        var runner = builder.ApplicationServices.GetRequiredService<IStartupModuleRunner>();
        runner.Initialize(builder.ApplicationServices);
        return builder;
    }
}