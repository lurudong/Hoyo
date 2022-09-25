using System.Reflection;

namespace Hoyo.AutoDependencyInjectionModule;

public static class ReflectHelper
{
    public static Assembly[] GetAssemblies() => AppDomain.CurrentDomain.GetAssemblies();
}
