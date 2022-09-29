using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;
/// <summary>
/// ApplicationInitialization扩展
/// </summary>
public static class ApplicationInitializationExtensions
{
    /// <summary>
    /// 获取应用程序构建器
    /// </summary>
    /// <param name="applicationContext"></param>
    /// <returns></returns>
    public static IApplicationBuilder GetApplicationBuilder(this ApplicationContext applicationContext) => applicationContext.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
}