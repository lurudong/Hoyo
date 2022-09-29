using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;

namespace example.net7.api;

/// <summary>
/// 自动注入模块测试
/// </summary>
public class TestModule : AppModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        var test = context.Services.GetService<MyTestModule>();
        test?.Show();
    }
}

/// <summary>
/// 测试模块
/// </summary>
public class MyTestModule : ITest
{
    /// <summary>
    /// Show
    /// </summary>
    public void Show()
    {
        Console.WriteLine("测试自动注入Test");
    }
}
/// <summary>
/// 测试
/// </summary>
//[IgnoreDependency]

public interface ITest : IScopedDependency
{
    /// <summary>
    /// Show函数
    /// </summary>
    void Show();
}
/// <summary>
/// 测试
/// </summary>
[DependencyInjection(ServiceLifetime.Singleton)]
public class Test2
{
    /// <summary>
    /// Show
    /// </summary>
    public void Show()
    {
        Console.WriteLine("测试自动注入Test2");
    }
}