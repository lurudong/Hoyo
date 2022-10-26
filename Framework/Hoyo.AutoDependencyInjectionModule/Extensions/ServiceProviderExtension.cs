using Hoyo.AutoDependencyInjectionModule.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hoyo.AutoDependencyInjectionModule.Extensions;
/// <summary>
/// ServiceProvider扩展
/// </summary>
public static class ServiceProviderExtension
{
    /// <summary>
    /// 获取指定类型的日志对象
    /// </summary>
    /// <typeparam name="T">非静态强类型</typeparam>
    /// <returns>日志对象</returns>
    public static ILogger<T> GetLogger<T>(this IServiceProvider provider) => provider.GetService<ILoggerFactory>()!.CreateLogger<T>();

    /// <summary>
    ///获取ILogger对象
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ILogger GetLogger(this IServiceProvider provider, Type type) => provider.GetService<ILoggerFactory>()!.CreateLogger(type);
    /// <summary>
    /// 获取实例
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    public static object? GetInstance(this IServiceProvider provider, ServiceDescriptor descriptor) =>
        descriptor.ImplementationInstance ?? (descriptor.ImplementationType is { } type
            ? provider.GetServiceOrCreateInstance(type)
            : descriptor.ImplementationFactory?.Invoke(provider));

    /// <summary>
    /// 获取日志记录器
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ILogger GetLogger(this ILazyServiceProvider provider, Type type) => provider.LazyGetService<ILoggerFactory>().CreateLogger(type);
    /// <summary>
    /// 获取服务或者创建实例
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static object GetServiceOrCreateInstance(this IServiceProvider provider, Type type) => ActivatorUtilities.GetServiceOrCreateInstance(provider, type);
    /// <summary>
    /// 创建实例
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="type"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static object CreateInstance(this IServiceProvider provider, Type type, params object[] arguments) => ActivatorUtilities.CreateInstance(provider, type, arguments);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="provider"></param>
    /// <param name="action"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void GetService<T>(this IServiceProvider provider, Action<T> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        var t = provider.GetService<T>();
        if (t == null) throw new ArgumentNullException(nameof(action));
        action(t);
    }
    /// <summary>
    /// 创建范围
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="callback"></param>
    public static void CreateScoped(this IServiceProvider provider, Action<IServiceProvider> callback)
    {
        using var scope = provider.CreateScope();
        callback(scope.ServiceProvider);
    }
}
