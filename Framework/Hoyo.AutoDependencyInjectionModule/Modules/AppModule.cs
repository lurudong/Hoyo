using System.Reflection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

public class AppModule : IAppModule
{
    public bool Enable { get; set; } = true;

    private ConfigureServicesContext? _configureServicesContext;
    public virtual void ApplicationInitialization(ApplicationContext context) { }

    public virtual void ConfigureServices(ConfigureServicesContext context) { }

    protected internal ConfigureServicesContext ConfigureServicesContext
    {
        get => _configureServicesContext is null
                ? throw new($"{nameof(ConfigureServicesContext)}仅适用于{nameof(ConfigureServices)}方法。")
                : _configureServicesContext;
        internal set => _configureServicesContext = value;
    }

    /// <summary>
    /// 获取模块程序集
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    public Type[] GetDependedTypes(Type? moduleType = null)
    {
        if (moduleType is null) moduleType = GetType();
        var dependedTypes = moduleType.GetCustomAttributes().OfType<IDependedTypesProvider>().ToArray();
        if (dependedTypes.Length == 0) return Array.Empty<Type>();
        List<Type> dependList = new();
        foreach (var dependedType in dependedTypes)
        {
            var dependeds = dependedType.GetDependedTypes();
            if (dependeds.Length == 0) continue;
            dependList.AddRange(dependeds);
            foreach (Type type in dependeds)
            {
                dependList.AddRange(GetDependedTypes(type));
            }
        }
        return dependList.Distinct().ToArray();
    }

    /// <summary>
    /// 判断是否是模块
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsAppModule(Type type)
    {
        var typeInfo = type.GetTypeInfo();
        return typeInfo.IsClass &&
             !typeInfo.IsAbstract &&
             !typeInfo.IsGenericType &&
             typeof(IAppModule).GetTypeInfo().IsAssignableFrom(type);
    }
}