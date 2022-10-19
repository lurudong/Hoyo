using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.AutoDependencyInjectionModule.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;
/// <summary>
/// LazyServiceProvider
/// </summary>
public class LazyServiceProvider : ILazyServiceProvider, ITransientDependency
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    private Dictionary<Type, object> CacheServices { get; set; } = new();
    /// <summary>
    /// IServiceProvider
    /// </summary>
    private IServiceProvider ServiceProvider { get; set; }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    public LazyServiceProvider(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LazyGetRequiredService<T>() => (T)LazyGetRequiredService(typeof(T));
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public object LazyGetRequiredService(Type serviceType) => CacheServices.GetOrAdd(serviceType, type => type is null
        ? throw new ArgumentNullException(nameof(type))
        : ServiceProvider.GetRequiredService(type));
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LazyGetService<T>() => (T)LazyGetService(typeof(T));
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public object LazyGetService(Type serviceType) => CacheServices.GetOrAdd(serviceType, type => type is null
        ? throw new ArgumentNullException(nameof(type))
        : ServiceProvider.GetService(type)!);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T LazyGetService<T>(T defaultValue) => (T)LazyGetService(typeof(T), defaultValue!);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public object LazyGetService(Type serviceType, object defaultValue) => LazyGetService(serviceType);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory) => CacheServices.GetOrAdd(serviceType, _ => factory(ServiceProvider));
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory"></param>
    /// <returns></returns>
    public T LazyGetService<T>(Func<IServiceProvider, object> factory) => (T)LazyGetService(typeof(T), factory);
}
