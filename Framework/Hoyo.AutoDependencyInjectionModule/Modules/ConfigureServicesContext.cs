using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

/// <summary>
/// 自定义配置服务上下文
/// </summary>
public class ConfigureServicesContext
{
    /// <summary>
    /// IServiceCollection
    /// </summary>
    public IServiceCollection Services { get; }
    /// <summary>
    /// 配置服务上下文
    /// </summary>
    /// <param name="services"></param>
    public ConfigureServicesContext(IServiceCollection services) => Services = services;
}