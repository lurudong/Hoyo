namespace Hoyo.AutoDependencyInjectionModule.Modules;

/// <summary>
/// 被依赖的类型提供方
/// </summary>
public interface IDependedTypesProvider
{
    /// <summary>
    /// 得到依赖类型集合
    /// </summary>
    /// <returns></returns>
    Type[] GetDependedTypes();
}