using Hoyo.Extensions;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

/// <summary>
/// 自定义应用上下文
/// </summary>
public class ApplicationContext : IServiceProviderAccessor
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    public ApplicationContext(IServiceProvider serviceProvider)
    {
        serviceProvider.NotNull(nameof(serviceProvider));
        ServiceProvider = serviceProvider;
    }
    /// <summary>
    /// IServiceProvider
    /// </summary>
    public IServiceProvider ServiceProvider { get; set; }
}