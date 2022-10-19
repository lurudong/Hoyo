using Hoyo.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hoyo.AutoDependencyInjectionModule.Extensions;
/// <summary>
/// IServiceCollection扩展
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// 得到注入服务
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static TType? GetService<TType>(this IServiceCollection services) => services.BuildServiceProvider().GetService<TType>();
    /// <summary>
    /// RegisterAssemblyTypes
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="assemblies">assemblies</param>
    /// <returns>services</returns>
    public static IServiceCollection RegisterAssemblyTypes(this IServiceCollection services, params Assembly[] assemblies) => RegisterAssemblyTypes(services, null, ServiceLifetime.Singleton, assemblies);

    /// <summary>
    /// RegisterAssemblyTypes
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="serviceLifetime">service lifetime</param>
    /// <param name="assemblies">assemblies</param>
    /// <returns>services</returns>
    public static IServiceCollection RegisterAssemblyTypes(this IServiceCollection services, ServiceLifetime serviceLifetime, params Assembly[] assemblies) => RegisterAssemblyTypes(services, null, serviceLifetime, assemblies);

    /// <summary>
    /// RegisterAssemblyTypes
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="typesFilter">filter types to register</param>
    /// <param name="assemblies">assemblies</param>
    /// <returns>services</returns>
    public static IServiceCollection RegisterAssemblyTypes(this IServiceCollection services, Func<Type, bool> typesFilter, params Assembly[] assemblies) =>
        RegisterAssemblyTypes(services, typesFilter, ServiceLifetime.Singleton, assemblies);

    /// <summary>
    /// RegisterAssemblyTypes
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="typesFilter">filter types to register</param>
    /// <param name="serviceLifetime">service lifetime</param>
    /// <param name="assemblies">assemblies</param>
    /// <returns>services</returns>
    private static IServiceCollection RegisterAssemblyTypes(this IServiceCollection services, Func<Type, bool>? typesFilter, ServiceLifetime serviceLifetime, params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            assemblies = ReflectHelper.GetAssemblies();
        }
        var types = assemblies.Select(assembly => assembly.GetExportedTypes()).SelectMany(t => t);
        if (typesFilter is not null)
        {
            types = types.Where(typesFilter);
        }
        foreach (var type in types)
        {
            services.Add(new(type, type, serviceLifetime));
        }
        return services;
    }

    /// <summary>
    /// RegisterTypeAsImplementedInterfaces
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="assemblies">assemblies</param>
    /// <returns>services</returns>
    public static IServiceCollection RegisterAssemblyTypesAsImplementedInterfaces(this IServiceCollection services, params Assembly[] assemblies) =>
        RegisterAssemblyTypesAsImplementedInterfaces(services, typesFilter: null, ServiceLifetime.Singleton, assemblies);

    /// <summary>
    /// RegisterTypeAsImplementedInterfaces
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="serviceLifetime">service lifetime</param>
    /// <param name="assemblies">assemblies</param>
    /// <returns>services</returns>
    public static IServiceCollection RegisterAssemblyTypesAsImplementedInterfaces(this IServiceCollection services, ServiceLifetime serviceLifetime, params Assembly[] assemblies)
        => RegisterAssemblyTypesAsImplementedInterfaces(services, typesFilter: null, serviceLifetime, assemblies);

    /// <summary>
    /// RegisterTypeAsImplementedInterfaces, singleton by default
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="typesFilter">filter types to register</param>
    /// <param name="assemblies">assemblies</param>
    /// <returns>services</returns>
    public static IServiceCollection RegisterAssemblyTypesAsImplementedInterfaces(this IServiceCollection services, Func<Type, bool> typesFilter, params Assembly[] assemblies)
        => RegisterAssemblyTypesAsImplementedInterfaces(services, typesFilter: typesFilter, ServiceLifetime.Singleton, assemblies);

    /// <summary>
    /// RegisterTypeAsImplementedInterfaces
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="typesFilter">filter types to register</param>
    /// <param name="serviceLifetime">service lifetime</param>
    /// <param name="assemblies">assemblies</param>
    /// <returns>services</returns>
    private static IServiceCollection RegisterAssemblyTypesAsImplementedInterfaces(this IServiceCollection services, Func<Type, bool>? typesFilter, ServiceLifetime serviceLifetime, params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            assemblies = ReflectHelper.GetAssemblies();
        }
        var types = assemblies.Select(assembly => assembly.GetExportedTypes()).SelectMany(t => t);
        if (typesFilter is not null)
        {
            types = types.Where(typesFilter);
        }
        foreach (var type in types)
        {
            foreach (var implementedInterface in type.GetImplementedInterfaces())
            {
                services.Add(new(implementedInterface, type, serviceLifetime));
            }
        }
        return services;
    }

    /// <summary>
    /// RegisterTypeAsImplementedInterfaces
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="type">type</param>
    /// <param name="serviceLifetime">service lifetime</param>
    /// <returns>services</returns>
    public static IServiceCollection RegisterTypeAsImplementedInterfaces(this IServiceCollection services, Type type, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        foreach (var interfaceType in type.GetImplementedInterfaces())
        {
            services.Add(new(interfaceType, type, serviceLifetime));
        }
        return services;
    }

    /// <summary>
    /// 得到注入服务
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    private static TType? GetBuildService<TType>(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        return provider.GetService<TType>();
    }

    /// <summary>
    /// 得到或添加Singleton服务
    /// </summary>
    /// <typeparam name="TServiceType"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static TServiceType GetOrAddSingletonService<TServiceType, TImplementation>(this IServiceCollection services) where TServiceType : class where TImplementation : class, TServiceType
    {
        var type = services.GetSingletonInstanceOrNull<TServiceType>();
        if (type is not null) return type;
        var provider = services.BuildServiceProvider();
        return (TServiceType)provider.GetInstance(new(typeof(TServiceType), typeof(TImplementation), ServiceLifetime.Singleton))!;
    }

    /// <summary>
    /// 得到或添加Singleton服务
    /// </summary>
    /// <typeparam name="TServiceType"></typeparam>

    public static TServiceType GetOrAddSingletonService<TServiceType>(this IServiceCollection services, Func<TServiceType> factory) where TServiceType : class
    {
        var serviceType = services.GetSingletonInstanceOrNull<TServiceType>();
        if (serviceType is not null) return serviceType;
        serviceType = factory();
        _ = services.AddSingleton(serviceType);
        return serviceType;
    }
    /// <summary>
    /// 获取IServiceCollection服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IConfiguration GetConfiguration(this IServiceCollection services) => services.GetBuildService<IConfiguration>() ?? throw new("未找到IConfiguration服务");

    /// <summary>
    /// 获取单例注册服务对象
    /// </summary>
    private static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services)
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T) && d.Lifetime == ServiceLifetime.Singleton);
        return descriptor?.ImplementationInstance is not null
            ? (T)descriptor.ImplementationInstance
            : descriptor?.ImplementationFactory is not null ? (T)descriptor.ImplementationFactory.Invoke(null!) : default;
    }
    /// <summary>
    /// 获取单列实例
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static T GetSingletonInstance<T>(this IServiceCollection services)
    {
        var service = services.GetSingletonInstanceOrNull<T>();
        return service is null ? throw new InvalidOperationException($"找不到singleton服务: {typeof(T).AssemblyQualifiedName}") : service;
    }

    #region New Module
    /// <summary>
    /// 试着添加对象适配器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static ObjectAccessor<T> TryAddObjectAccessor<T>(this IServiceCollection services) => services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>))
            ? services.GetSingletonInstance<ObjectAccessor<T>>()
            : services.AddObjectAccessor<T>();
    /// <summary>
    /// 添加对象适配器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    private static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services) => services.AddObjectAccessor(new ObjectAccessor<T>());
    /// <summary>
    /// 添加对象适配器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, T obj) => services.AddObjectAccessor(new ObjectAccessor<T>(obj));
    /// <summary>
    /// 添加对象适配器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="accessor"></param>
    /// <returns></returns>
    private static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, ObjectAccessor<T> accessor)
    {
        if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
        {
            throw new("在类型“{typeof(T).AssemblyQualifiedName)}”之前注册了对象: ");
        }
        //Add to the beginning for fast retrieve
        services.Insert(0, ServiceDescriptor.Singleton(typeof(ObjectAccessor<T>), accessor));
        services.Insert(0, ServiceDescriptor.Singleton(typeof(IObjectAccessor<T>), accessor));
        return accessor;
    }
    /// <summary>
    /// 获取对象或者Null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    private static T? GetObjectOrNull<T>(this IServiceCollection services) where T : class => services.GetSingletonInstanceOrNull<IObjectAccessor<T>>()?.Value;
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static T GetObject<T>(this IServiceCollection services) where T : class => services.GetObjectOrNull<T>() ?? throw new($"找不到的对象 {typeof(T).AssemblyQualifiedName} 服务。请确保您以前使用过AddObjectAccessor！");
    /// <summary>
    /// 从工厂创建服务适配器
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceProvider BuildServiceProviderFromFactory(this IServiceCollection services)
    {
        foreach (var service in services)
        {
            var factoryInterface = service.ImplementationInstance?.GetType()
                .GetTypeInfo()
                .GetInterfaces()
                .FirstOrDefault(i => i.GetTypeInfo().IsGenericType &&
                                     i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>));
            if (factoryInterface is null) continue;
            var containerBuilderType = factoryInterface.GenericTypeArguments[0];
            return (IServiceProvider)typeof(ServiceCollectionExtension)
                .GetTypeInfo()
                .GetMethods()
                .Single(m => m.Name == nameof(BuildServiceProviderFromFactory) && m.IsGenericMethod)
                .MakeGenericMethod(containerBuilderType)
                .Invoke(null, new object[] { services, null! })!;
        }
        return services.BuildServiceProvider();
    }

    #endregion New Module

    /// <summary>
    /// 获取指定key的值,如没有则设置并获取默认值
    /// </summary>
    /// <typeparam name="TKey">key的类型</typeparam>
    /// <typeparam name="TValue">value的类型</typeparam>
    /// <param name="dict">字典</param>
    /// <param name="key">key</param>
    /// <param name="func">默认值委托</param>
    /// <returns></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> func) => dict.TryGetValue(key, out var obj) ? obj : (dict[key] = func(key));
}
