using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Hoyo.Mongo;
public class HoyoMongoOptions
{
    /// <summary>
    /// ConventionPackOptions Action
    /// </summary>
    public Action<ConventionPackOptions>? ConventionPackOptionsAction { get; set; } = null;
    /// <summary>
    /// RegistryConventionPack first
    /// </summary>
    public bool? First { get; set; } = true;
    public Dictionary<string, ConventionRegistryConfig> ConventionRegistry { get; private set; } = new()
    {
        {
            "hoyopack",
            new()
            {
                Conventions = new()
                {
                    new CamelCaseElementNameConvention(), // 驼峰名称格式
                    new IgnoreExtraElementsConvention(true), //
                    new NamedIdMemberConvention("Id","ID"), // _id映射为实体中的ID或者Id
                    new EnumRepresentationConvention(BsonType.String) // 将枚举类型存储为字符串格式
                },
                Filter = _ => true
            }
        }
    };
    /// <summary>
    /// 添加自己的一些Convention配置,用于设置mongodb序列化反序列化的一些表现.
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="config">ConventionRegistryConfig</param>
    public void AppendConventionRegistry(string name, ConventionRegistryConfig config) => ConventionRegistry.Add(name, config);
}

public class ConventionRegistryConfig
{
    public ConventionPack Conventions { get; set; } = new();
    public Func<Type, bool>? Filter { get; set; } = null;
}