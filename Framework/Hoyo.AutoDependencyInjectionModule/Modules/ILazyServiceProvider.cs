namespace Hoyo.AutoDependencyInjectionModule.Modules;

/// <summary>
/// 懒加载服务提供者
/// </summary>
public interface ILazyServiceProvider
{
    /// <summary>
    /// 获取所需服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T LazyGetRequiredService<T>();
    /// <summary>
    /// 获取所需服务
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    object LazyGetRequiredService(Type serviceType);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T LazyGetService<T>();
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    object LazyGetService(Type serviceType);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    T LazyGetService<T>(T defaultValue);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    object LazyGetService(Type serviceType, object defaultValue);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory);
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory"></param>
    /// <returns></returns>
    T LazyGetService<T>(Func<IServiceProvider, object> factory);
}
