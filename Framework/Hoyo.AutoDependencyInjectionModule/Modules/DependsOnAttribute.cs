namespace Hoyo.AutoDependencyInjectionModule.Modules;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute : Attribute, IDependedTypesProvider
{
    public DependsOnAttribute(params Type[] dependedTypes) => DependedTypes = dependedTypes ?? Array.Empty<Type>();

    /// <summary>
    /// 依赖类型集合
    /// </summary>
    private Type[] DependedTypes { get; }

    /// <summary>
    /// 得到依赖类型集合
    /// </summary>
    /// <returns></returns>
    public Type[] GetDependedTypes() => DependedTypes;
}