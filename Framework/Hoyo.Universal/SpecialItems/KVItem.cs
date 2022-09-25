namespace Hoyo.Universal;

public class KVItem
{
    public KVItem() { }
    public KVItem(string k, string v) { K = k; V = v; }
    public string K { get; set; } = string.Empty;
    public string V { get; set; } = string.Empty;
    public override string ToString() => V;
    public bool IsIntegrated() => !string.IsNullOrEmpty(K) && !string.IsNullOrEmpty(V);
    protected bool Equals(KVItem other) => K == other.K && V == other.V;
}
public class KVItem<T>
{
    public string K { get; set; } = string.Empty;
    public T? V { get; set; }
}