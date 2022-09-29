using System.Reflection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;
/// <summary>
/// AppModule
/// </summary>
public class AppModule : IAppModule
{
    /// <summary>
    /// 是否启用,默认为true
    /// </summary>
    public bool Enable { get; set; } = true;

    private ConfigureServicesContext? _configureServicesContext;
    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    public virtual void ApplicationInitialization(ApplicationContext context) { }
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public virtual void ConfigureServices(ConfigureServicesContext context) { }
    /// <summary>
    /// 配置服务上下文
    /// </summary>
    protected internal ConfigureServicesContext ConfigureServicesContext
    {
        get => _configureServicesContext ?? throw new($"{nameof(ConfigureServicesContext)}仅适用于{nameof(ConfigureServices)}方法。");
        internal set => _configureServicesContext = value;
    }

    /// <summary>
    /// 获取模块程序集
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    public Type[] GetDependedTypes(Type? moduleType = null)
    {
        moduleType ??= GetType();
        var dependedTypes = moduleType.GetCustomAttributes().OfType<IDependedTypesProvider>().ToArray();
        if (dependedTypes.Length == 0) return Array.Empty<Type>();
        List<Type> dependList = new();
        foreach (var dependedType in dependedTypes)
        {
            var dependeds = dependedType.GetDependedTypes();
            if (dependeds.Length == 0) continue;
            dependList.AddRange(dependeds);
            foreach (var type in dependeds)
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