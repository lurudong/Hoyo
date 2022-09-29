using Microsoft.Extensions.DependencyModel;
using System.Reflection;
#if !NETSTANDARD
using System.Runtime.Loader;
#endif

namespace Hoyo.Extensions;

/// <summary>
/// 程序集帮助类
/// </summary>
public static class AssemblyHelper
{
#if !NETSTANDARD
    /// <summary>
    /// 根据程序集名字得到程序集
    /// </summary>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>

    public static IEnumerable<Assembly> GetAssembliesByName(params string[] assemblyNames)
    {
        var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath; //获取项目路径
        return assemblyNames.Select(o => AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(basePath, $"{o}.dll")));
    }
#endif

    private static readonly string[] Filters = { "dotnet-", "Microsoft.", "mscorlib", "netstandard", "System", "Windows" };

    private static Assembly[]? _allAssemblies;
    private static Type[]? _allTypes;

    /// <summary>
    /// 获取 所有程序集
    /// </summary>
    public static Assembly[] AllAssemblies
    {
        get
        {
            if (_allAssemblies is null)
            {
                Init();
            }
            return _allAssemblies!;
        }
    }

    /// <summary>
    /// 获取 所有类型
    /// </summary>
    public static Type[] AllTypes
    {
        get
        {
            if (_allTypes is null)
            {
                Init();
            }
            return _allTypes!;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        _allAssemblies = DependencyContext.Default?.GetDefaultAssemblyNames().Where(o => o.Name != null && !Filters.Any(o.Name.StartsWith)).Select(Assembly.Load).ToArray();
        _allTypes = _allAssemblies?.SelectMany(m => m.GetTypes()).ToArray();
    }

    /// <summary>
    /// 查找指定条件的类型
    /// </summary>
    public static Type[] FindTypes(Func<Type, bool> predicate) => AllTypes.Where(predicate).ToArray();
    /// <summary>
    /// 查找所有指定特性标记的类型
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static Type[] FindTypesByAttribute<TAttribute>() where TAttribute : Attribute => FindTypesByAttribute(typeof(TAttribute));
    /// <summary>
    /// 查找所有指定特性标记的类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type[] FindTypesByAttribute(Type type) => AllTypes.Where(a => a.IsDefined(type, true)).Distinct().ToArray();
    /// <summary>
    /// 查找指定条件的类型
    /// </summary>
    public static Assembly[] FindAllItems(Func<Assembly, bool> predicate) => AllAssemblies.Where(predicate).ToArray();
}