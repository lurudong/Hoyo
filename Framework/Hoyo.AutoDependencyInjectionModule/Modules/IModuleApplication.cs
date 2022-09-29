﻿using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;
/// <summary>
/// 模块化应用
/// </summary>
public interface IModuleApplication : IDisposable
{
    /// <summary>
    /// 启动模块类型
    /// </summary>
    Type StartupModuleType { get; }
    /// <summary>
    /// IServiceCollection
    /// </summary>
    IServiceCollection Services { get; }
    /// <summary>
    /// IServiceProvider
    /// </summary>
    IServiceProvider? ServiceProvider { get; }
    /// <summary>
    /// Modules
    /// </summary>
    IReadOnlyList<IAppModule> Modules { get; }
    /// <summary>
    /// Source
    /// </summary>
    List<IAppModule> Source { get; }
}