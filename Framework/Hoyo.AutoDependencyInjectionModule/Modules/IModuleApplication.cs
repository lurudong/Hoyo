using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

public interface IModuleApplication : IDisposable
{
    Type StartupModuleType { get; }
    IServiceCollection Services { get; }
    IServiceProvider? ServiceProvider { get; }
    IReadOnlyList<IAppModule> Modules { get; }
    List<IAppModule> Source { get; }
}