using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

public class ModuleApplicationBase : IModuleApplication
{
    public Type StartupModuleType { get; set; }

    public IServiceCollection Services { get; set; }

    public IServiceProvider? ServiceProvider { get; private set; }

    /// <summary>
    /// 模块接口容器
    /// </summary>
    public IReadOnlyList<IAppModule> Modules { get; set; }

    public List<IAppModule> Source { get; protected set; }

    public ModuleApplicationBase(Type startupModuleType, IServiceCollection services)
    {
        ServiceProvider = null;
        StartupModuleType = startupModuleType;
        Services = services;
        _ = services.AddSingleton<IModuleApplication>(this);
        _ = services.TryAddObjectAccessor<IServiceProvider>();
        Source = GetEnabledAllModule(services);
        Modules = LoadModules();
    }

    protected virtual List<IAppModule> GetEnabledAllModule(IServiceCollection services)
    {
        var types = AssemblyHelper.FindTypes(o => AppModule.IsAppModule(o));
        var modules = types.Select(o => CreateModule(services, o)).Where(c => c is not null);
        //var notnullmodules = new List<IAppModule>();
        //foreach (var module in modules)
        //{
        //    if (module is null) continue;
        //    notnullmodules.Add(module);
        //}
        return modules.Distinct().ToList()!;
    }

    protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
    }

    /// <summary>
    /// 获取所有需要加载的模块
    /// </summary>
    /// <returns></returns>
    protected virtual IReadOnlyList<IAppModule> LoadModules()
    {
        List<IAppModule> modules = new();
        var module = Source.FirstOrDefault(o => o.GetType() == StartupModuleType);
        if (module is null) throw new($"类型为“{StartupModuleType.FullName}”的模块实例无法找到");
        modules.Add(module);
        var dependeds = module.GetDependedTypes();
        foreach (var dependType in dependeds.Where(o => AppModule.IsAppModule(o)))
        {
            var dependModule = Source.Find(m => m.GetType() == dependType);
            if (dependModule is null) continue;
            //if (dependModule is null)
            //{
            //    //throw new($"加载模块{module.GetType().FullName}时无法找到依赖模块{dependType.FullName}");
            //}
            _ = modules.AddIfNotContains(dependModule);
        }
        return modules;
    }

    /// <summary>
    /// 创建模块
    /// </summary>
    /// <param name="services"></param>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    private static IAppModule? CreateModule(IServiceCollection services, Type moduleType)
    {
        var module = (IAppModule)Expression.Lambda(Expression.New(moduleType)).Compile().DynamicInvoke()!;
        if (module is null)
        {
            var msg = nameof(module);
            throw new ArgumentNullException(msg);
        }
        if (!module.Enable) return null;
        _ = services.AddSingleton(moduleType, module);
        return module;
    }

    public virtual void Dispose() => GC.SuppressFinalize(this);
}