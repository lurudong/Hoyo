using Hoyo.AutoDependencyInjectionModule.Extensions;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

/// <summary>
/// 自定义应用上下文
/// </summary>
public class ApplicationContext : IServiceProviderAccessor
{
    public ApplicationContext(IServiceProvider serviceProvider)
    {
        serviceProvider.NotNull(nameof(serviceProvider));
        ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; set; }
}