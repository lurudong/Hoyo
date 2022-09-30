namespace Hoyo.Universal;

/// <summary>
/// ReferenceItem,通常用来保存关联的一些业务信息
/// </summary>
public class ReferenceItem
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="rid"></param>
    public ReferenceItem(string rid) { Rid = rid; }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="name"></param>
    public ReferenceItem(string rid, string name) { Rid = rid; Name = name; }
    /// <summary>
    /// 标识(引用Id)
    /// </summary>
    public string Rid { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 对比两个对象是否相同,通常认为Rid一致就是同一个对象,不对Name校验
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool Equal(ReferenceItem target) => Rid == target.Rid;
    /// <summary>
    /// ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;
}