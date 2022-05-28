namespace Hoyo.AutoDependencyInjectionModule;

public interface IObjectAccessor<TType>
{
    TType Value { get; set; }
}

public class ObjectAccessor<TType> : IObjectAccessor<TType?>
{
    public ObjectAccessor() { }

    public ObjectAccessor(TType obj) => Value = obj;

    public TType? Value { get; set; }
}