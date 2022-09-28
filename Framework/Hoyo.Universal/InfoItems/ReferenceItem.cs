namespace Hoyo.Universal;

public class ReferenceItem : IReferenceItem
{
    public ReferenceItem() { }
    public ReferenceItem(string rid) { Rid = rid; }
    public ReferenceItem(string rid, string name) { Rid = rid; Name = name; }
    /// <summary>
    /// 标识(引用Id)
    /// </summary>
    public string Rid { get; set; } = string.Empty;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public ReferenceItem GetReferenceItem() => new(Rid, Name);
    public bool Equal(ReferenceItem target) => Rid == target.Rid;
    public bool IsIntegrated() => !string.IsNullOrWhiteSpace(Rid) && !string.IsNullOrWhiteSpace(Name);
    public override string ToString() => Name;
}