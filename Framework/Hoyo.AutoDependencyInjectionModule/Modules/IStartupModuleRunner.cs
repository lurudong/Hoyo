using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

/// <summary>
///
/// </summary>
public interface IStartupModuleRunner : IModuleApplication
{
    void ConfigureServices(IServiceCollection services);

    void Initialize(IServiceProvider service);
}