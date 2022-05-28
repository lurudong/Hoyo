namespace Hoyo.Universal;

public class IdNameItem : IGetReferenceItem
{
    public IdNameItem() { }
    public IdNameItem(string id, string name) { Id = id; Name = name; }
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IdNameItem GetIdNameItem() => new(Id, Name);
    public ReferenceItem GetReferenceItem() => new(Id, Name);
    public override string ToString() => Name;
}