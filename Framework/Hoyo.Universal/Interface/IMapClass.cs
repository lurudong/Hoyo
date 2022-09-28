namespace Hoyo.Universal;
public interface IMapClass<out T>
{
    T GetMapClass(Action<T>? action = null);
}
public interface IMapClassOnly<out T>
{
    T GetMapClass();
}
public interface IFromMapClass<in TFrom, out Tto>
{
    Tto FromMapClass(TFrom tFrom);
}
public interface IMapClassFrom<T>
{
    T GetMapClassFrom(T from, Action<T>? action = null);
}