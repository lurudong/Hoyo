using Hoyo.AutoDependencyInjectionModule.DependencyInjectionModule;
using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;

namespace example.api;

public class TestModule : AppModule
{

    public override void ConfigureServices(ConfigureServicesContext context)
    {
        var test = context.Services.GetService<Test>();
        test?.Show();
    }
}

public class Test : ITest
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