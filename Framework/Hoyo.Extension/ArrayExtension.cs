﻿namespace Hoyo.Extensions;
/// <summary>
/// 数组扩展
/// </summary>
public static class ArrayExtension
{
    /// <summary>
    /// 获取数组中随机的一个数
    /// </summary>
    /// <typeparam name="T">数组对象类型</typeparam>
    /// <param name="array">数组对象</param>
    /// <returns>随机返回数组中的一个对象</returns>
    public static T GetRandom<T>(this T[] array)
    {
        if (array is null || array.Length == 0) throw new("array must be not null");
        var random = new Random();
        return array[random.Next(0, array.Length)];
    }

    /// <summary>
    /// 获取数组的子数组
    /// </summary>
    /// <typeparam name="T">数组对象类型</typeparam>
    /// <param name="array">数组对象</param>
    /// <param name="startIndex">起始位置</param>
    /// <param name="length">子数组长度,若是长度大于起始位置至末位元素数量,返回起始元素至末位元素子数组</param>
    /// <returns>子数组对象</returns>
    public static T[] SubArray<T>(this T[] array, int startIndex, int length = -1) => length == -1 || length - 1 >= array.Length - 1 - startIndex
            ? array.Skip(startIndex).ToArray()
            : array.Skip(startIndex).Take(length).ToArray();

    /// <summary>
    /// 向数组中新增一个新元素,并返回新数组.
    /// </summary>
    /// <typeparam name="T">数组对象类型</typeparam>
    /// <param name="array">被添加数组对象</param>
    /// <param name="obj">新元素</param>
    /// <returns>新增元素后的新数组</returns>
    public static T[] Push<T>(this IEnumerable<T> array, T obj)
    {
        if (obj is null) throw new("obj must be not null");
        var lt = array.ToList();
        lt.Add(obj);
        return lt.ToArray();
    }

    /// <summary>
    /// 向数组中新增一组新元素,并返回新数组
    /// </summary>
    /// <typeparam name="T">数组对象类型</typeparam>
    /// <param name="array">被添加数组对象</param>
    /// <param name="obj">新元素数组</param>
    /// <returns>新增元素后的新数组</returns>
    public static T[] PushRange<T>(this IEnumerable<T> array, T[] obj)
    {
        if (obj is null) throw new("obj must be not null");
        var lt = array.ToList();
        lt.AddRange(obj);
        return lt.ToArray();
    }

    /// <summary>
    /// 删除数组中最后一个元素,并返回该元素和删除数据后的新数组
    /// </summary>
    /// <typeparam name="T">数组对象类型</typeparam>
    /// <param name="array">被操作数组</param>
    /// <returns>被删除对象,和删除元素后的数组</returns>
    public static Tuple<T, T[]> Pop<T>(this IEnumerable<T> array)
    {
        var lt = array.ToList();
        var t = lt.Last();
        _ = lt.Remove(t);
        var temp = lt.ToArray();
        return new(t, temp);
    }
}