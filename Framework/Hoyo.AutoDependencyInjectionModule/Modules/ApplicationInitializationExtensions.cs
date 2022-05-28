using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

public static class ApplicationInitializationExtensions
{
    public static IApplicationBuilder GetApplicationBuilder(this ApplicationContext applicationContext) => applicationContext.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
}