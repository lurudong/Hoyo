namespace Hoyo.Universal;

public class IdRidNameItem : IGetReferenceItem
{
    public string Id { get; set; } = string.Empty;
    public string Rid { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ReferenceItem GetReferenceItem() => new(Rid, Name);
    public override string ToString() => Name;
}