using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.AutoDependencyInjectionModule.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

public class LazyServiceProvider : ILazyServiceProvider, ITransientDependency
{
    protected Dictionary<Type, object> CacheServices { get; set; } = new();
    protected IServiceProvider ServiceProvider { get; set; }

    public LazyServiceProvider(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

    public T LazyGetRequiredService<T>() => (T)LazyGetRequiredService(typeof(T));

    public object LazyGetRequiredService(Type serviceType) => CacheServices.GetOrAdd(serviceType, serviceType => ServiceProvider.GetRequiredService(serviceType));

    public T LazyGetService<T>() => (T)LazyGetService(typeof(T));

    public object LazyGetService(Type serviceType) => CacheServices!.GetOrAdd(serviceType, serviceType => ServiceProvider.GetService(serviceType))!;

    public T LazyGetService<T>(T defaultValue) => (T)LazyGetService(typeof(T), defaultValue!);

    public object LazyGetService(Type serviceType, object defaultValue) => LazyGetService(serviceType) ?? defaultValue;

    public object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory) => CacheServices.GetOrAdd(serviceType, serviceType => factory(ServiceProvider));

    public T LazyGetService<T>(Func<IServiceProvider, object> factory) => (T)LazyGetService(typeof(T), factory);
}
