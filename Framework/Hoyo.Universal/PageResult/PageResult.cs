namespace Hoyo.Universal;
public class PageResult
{
    /// <summary>
    /// 泛型类型分页数据返回
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="total">数据总量</param>
    /// <param name="list">分页数据</param>
    /// <returns></returns>
    public static PageResult<T> Wrap<T>(long? total, IEnumerable<T> list) => new(total, list);
    /// <summary>
    /// 动态类型分页数据返回
    /// </summary>
    /// <param name="total">数据总量</param>
    /// <param name="list">分页数据</param>
    /// <returns></returns>
    public static PageResult<dynamic> WrapDynamic(long? total, IEnumerable<dynamic> list) => new(total, list);
}

public class PageResult<T>
{
    public PageResult(long? total, IEnumerable<T> list)
    {
        Total = total ?? 0;
        List = list;
    }
    public long Total { get; set; }
    public IEnumerable<T> List { get; set; }
}