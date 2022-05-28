namespace Hoyo.Universal;
public class PageInfo
{
    /// <summary>
    /// 页码
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// 每页数据量
    /// </summary>
    public int Size { get; set; }
}

public class KeywordPageInfo : PageInfo
{
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string? Key { get; set; }
}

public class KeywordIsEnablePageInfo : KeywordPageInfo
{
    /// <summary>
    /// 数据状态,null 表示所有,true 正常,false 禁用
    /// </summary>
    public bool? Enable { get; set; }
}