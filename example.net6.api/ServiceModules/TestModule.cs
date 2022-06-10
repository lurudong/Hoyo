using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;

namespace example.net6.api;

public class TestModule : AppModule
{

    public override void ConfigureServices(ConfigureServicesContext context)
    {
        var test = context.Services.GetService<MyTestModule>();
        test?.Show();
    }
}

public class MyTestModule : ITest
{
    public void Show()
    {
        Console.WriteLine("测试自动注入Test");
    }
}
//[IgnoreDependency]

public interface ITest : IScopedDependency
{
    void Show();
}

[DependencyInjection(ServiceLifetime.Scoped)]
public class Test2
{
    public void Show()
    {
        Console.WriteLine("测试自动注入Test2");
    }
}