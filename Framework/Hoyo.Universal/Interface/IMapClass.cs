namespace Hoyo.Universal;
public interface IMapClass<out T>
{
    T GetMapClass(Action<T>? action = null);
}
public interface IMapClassOnly<out T>
{
    T GetMapClass();
}
public interface IFromMapClass<in Tfrom, out Tto>
{
    Tto FromMapClass(Tfrom tfrom);
}
public interface IMapClassFrom<T>
{
    T GetMapClassFrom(T from, Action<T>? action = null);
}