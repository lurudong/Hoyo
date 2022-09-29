﻿using Hoyo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;

/// <summary>
/// IOC管理
/// </summary>
public class IocManage
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    private IServiceProvider? _provider;

    /// <summary>
    /// 服务集合
    /// </summary>
    private IServiceCollection? _services;

    /// <summary>
    /// 创建懒加载Ioc管理实例
    /// </summary>
    private static readonly Lazy<IocManage> InstanceLazy = new(() => new IocManage());
    /// <summary>
    /// IocManage实例
    /// </summary>
    public static IocManage Instance => InstanceLazy.Value;

    /// <summary>
    /// 设置应用程序服务提供者
    /// </summary>
    internal void SetApplicationServiceProvider(IServiceProvider provider)
    {
        provider.NotNull(nameof(provider));
        _provider = provider;
    }

    /// <summary>
    /// 设置应用程序服务集合
    /// </summary>
    public void SetServiceCollection(IServiceCollection services)
    {
        services.NotNull(nameof(services));
        _services = services;
    }
    /// <summary>
    /// 得到服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>

    public T? GetService<T>()
    {
        _provider.NotNull(nameof(_provider));
        _services.NotNull(nameof(_services));
        return _provider!.GetService<T>();
    }

    /// <summary>
    /// 得到日志记录
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public ILogger? GetLogger<T>() => _provider?.GetService<ILoggerFactory>()?.CreateLogger<T>();
}