using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

/// <summary>
/// 自定义配置服务上下文
/// </summary>
public class ConfigureServicesContext
{
    public IServiceCollection Services { get; }
    public ConfigureServicesContext(IServiceCollection services) => Services = services;
}