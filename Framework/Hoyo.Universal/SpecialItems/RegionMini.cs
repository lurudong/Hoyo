namespace Hoyo.Universal;
public class RegionMini : IIntegrate
{
    public List<string> Codes { get; set; } = new();
    public List<string> Names { get; set; } = new();
    public string Text => Names.Count > 0 ? string.Join("/", Names) : string.Empty;
    public override string ToString() => Text;
    public bool IsIntegrated() => Codes.Count == 3 && Names.Count == 3;
    public bool IsEmpty => Codes.Count == 0 || Names.Count == 0;
}