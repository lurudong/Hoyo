using Hoyo.AutoDependencyInjectionModule.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hoyo.AutoDependencyInjectionModule.Extensions;

public static class ServiceProviderExtension
{
    /// <summary>
    /// 获取指定类型的日志对象
    /// </summary>
    /// <typeparam name="T">非静态强类型</typeparam>
    /// <returns>日志对象</returns>
    public static ILogger<T> GetLogger<T>(this IServiceProvider provider) => provider.GetService<ILoggerFactory>()!.CreateLogger<T>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ILogger? GetLogger(this IServiceProvider provider, Type type) => provider.GetService<ILoggerFactory>()!.CreateLogger(type);

    public static object? GetInstance(this IServiceProvider provider, ServiceDescriptor descriptor) =>
        descriptor.ImplementationInstance is not null
            ? descriptor.ImplementationInstance
            : descriptor.ImplementationType is not null
            ? provider.GetServiceOrCreateInstance(descriptor.ImplementationType)
            : descriptor.ImplementationFactory is not null ? descriptor.ImplementationFactory(provider) : null;

    /// <summary>
    /// 获取日志记录器
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ILogger GetLogger(this ILazyServiceProvider provider, Type type) => provider.LazyGetService<ILoggerFactory>().CreateLogger(type);

    public static object GetServiceOrCreateInstance(this IServiceProvider provider, Type type) => ActivatorUtilities.GetServiceOrCreateInstance(provider, type);

    public static object CreateInstance(this IServiceProvider provider, Type type, params object[] arguments) => ActivatorUtilities.CreateInstance(provider, type, arguments);

    public static void GetService<T>(this IServiceProvider provider, Action<T> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        var t = provider.GetService<T>();
        if (t == null) throw new ArgumentNullException(nameof(action));
        action(t);
    }

    public static void CreateScoped(this IServiceProvider provider, Action<IServiceProvider> callback)
    {
        using var scope = provider.CreateScope();
        callback(scope.ServiceProvider);
    }
}
