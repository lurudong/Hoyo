namespace Hoyo.AutoDependencyInjectionModule.Modules;

/// <summary>
/// 定义模块加载接口
/// </summary>
public interface IAppModule : IApplicationInitialization
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    void ConfigureServices(ConfigureServicesContext context);
    /// <summary>
    /// 服务依赖集合
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    Type[] GetDependedTypes(Type? moduleType = null);
    /// <summary>
    /// 是否启用
    /// </summary>
    bool Enable { get; set; }
}