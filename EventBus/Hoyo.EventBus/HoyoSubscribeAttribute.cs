namespace Hoyo.EventBus;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class HoyoSubscribeAttribute : Attribute
{
    /// <summary>
    /// Topic or exchange route key name.
    /// </summary>
    public string Name { get; }
    public HoyoSubscribeAttribute(string name) => Name = name;
}
