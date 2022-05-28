namespace Hoyo.Universal;
public class KVItem
{
    public KVItem() { }
    public KVItem(string k, string v) { K = k; V = v; }
    public string K { get; set; } = string.Empty;
    public string V { get; set; } = string.Empty;
    public override string ToString() => V;
    public bool IsIntegrated() => !string.IsNullOrEmpty(K) && !string.IsNullOrEmpty(V);
    public override bool Equals(object? obj) => obj is KVItem objKV && objKV.K == K && objKV.V == V;
    public override int GetHashCode() => base.GetHashCode();
}
public class KVItem<T>
{
    public string K { get; set; } = string.Empty;
    public T? V { get; set; }
}