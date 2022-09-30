using System.Reflection;

namespace Hoyo.AutoDependencyInjectionModule;
/// <summary>
/// 反射助手
/// </summary>
public static class ReflectHelper
{
    /// <summary>
    /// 获取程序集
    /// </summary>
    /// <returns></returns>
    public static Assembly[] GetAssemblies() => AppDomain.CurrentDomain.GetAssemblies();
}
