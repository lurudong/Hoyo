namespace Hoyo.AutoDependencyInjectionModule;

/// <summary>
/// 对象存取器
/// </summary>
/// <typeparam name="TType"></typeparam>
public interface IObjectAccessor<TType>
{
    /// <summary>
    /// 值
    /// </summary>
    TType Value { get; set; }
}

/// <summary>
/// 对象存取器
/// </summary>
/// <typeparam name="TType"></typeparam>
public class ObjectAccessor<TType> : IObjectAccessor<TType?>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ObjectAccessor() { }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="obj"></param>
    public ObjectAccessor(TType obj) => Value = obj;
    /// <summary>
    /// 值
    /// </summary>
    public TType? Value { get; set; }
}