﻿namespace Hoyo.EventBus.Extensions;

public static class GenericTypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        var typeName = string.Empty;
        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }
        return typeName;
    }

    /// <summary>
    /// 获取通用类型名称
    /// </summary>
    /// <param name="object"></param>
    /// <returns></returns>
    public static string GetGenericTypeName(this object @object) => @object.GetType().GetGenericTypeName();
}
