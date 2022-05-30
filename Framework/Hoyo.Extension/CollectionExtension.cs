using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Hoyo.Extensions;

public static partial class CollectionExtension
{
    /// <summary>
    /// 把集合转成SqlIn
    /// </summary>
    /// <typeparam name="TSource">源</typeparam>
    /// <param name="values">要转换的值</param>
    /// <param name="separator">分割符</param>
    /// <param name="left">左边符</param>
    /// <param name="right">右边符</param>
    /// <returns>返回组装好的值，例如"'a','b'"</returns>
    public static string ToSqlIn<TSource>(this IEnumerable<TSource> values, string separator = ",", string left = "'", string right = "'")
    {
        StringBuilder sb = new();
        var enumerable = values as TSource[] ?? values.ToArray();
        if (!enumerable.Any())
        {
            return string.Empty;
        }
        enumerable.ToList().ForEach(o => _ = sb.Append($"{left}{o}{right}{separator}"));
        return sb.ToString()?.TrimEnd($"{separator}".ToCharArray())!;
    }
    /// <summary>
    /// WhereIf 扩展,可显著减少if的使用
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool condition) where TSource : IEnumerable
    {
        source.NotNullOrEmpty(nameof(source));
        predicate.NotNull(nameof(predicate));
        return condition ? source.Where(predicate) : source;
    }

    /// <summary>
    /// 将列表转换为树形结构（泛型无限递归）
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="list">数据</param>
    /// <param name="rootwhere">根条件</param>
    /// <param name="childswhere">节点条件</param>
    /// <param name="addchilds">添加子节点</param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static List<T> ToTree<T>(this List<T> list, Func<T, T, bool> rootwhere, Func<T, T, bool> childswhere, Action<T, IEnumerable<T>> addchilds, T? entity = default)
    {
        var treelist = new List<T>();
        //空树
        if (list == null || list.Count == 0)
        {
            return treelist;
        }
        if (!list.Any<T>(e => rootwhere(entity!, e)))
        {
            return treelist;
        }
        //树根
        if (list.Any<T>(e => rootwhere(entity!, e)))
        {
            treelist.AddRange(list.Where(e => rootwhere(entity!, e)));
        }
        //树叶
        foreach (var item in treelist)
        {
            if (list.Any(e => childswhere(item, e)))
            {
                var nodedata = list.Where(e => childswhere(item, e)).ToList();
                foreach (var child in nodedata)
                {
                    //添加子集
                    var data = list.ToTree(childswhere, childswhere, addchilds, child);
                    addchilds(child, data);
                }
                addchilds(item, nodedata);
            }
        }
        return treelist;
    }

    /// <summary>
    /// 添加一个元素若是元素不存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool AddIfNotContains<T>([NotNull] this ICollection<T> source, T item)
    {
        if (source.Contains(item))
        {
            return false;
        }
        source.Add(item);
        return true;
    }
}