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
    /// 是否使用本库提供的默认转换,默认为true
    /// </summary>
    public bool UseDefalutConventionRegistryConfig = true;
    /// <summary>
    /// 是否是第一次注册,重复注册会报错.
    /// </summary>
    public bool RegistryPackFirst = true;

    internal Dictionary<string, ConventionPack> ConventionRegistry { get; set; } = new()
    {
        {
            HoyoStatic.HoyoPack,
            new()
            {
                new CamelCaseElementNameConvention(), // 驼峰名称格式
                new IgnoreExtraElementsConvention(true), //
                new NamedIdMemberConvention("Id","ID"), // _id映射为实体中的ID或者Id
                new EnumRepresentationConvention(BsonType.String) // 将枚举类型存储为字符串格式
            }
        }
    };
    /// <summary>
    /// 添加自己的一些Convention配置,用于设置mongodb序列化反序列化的一些表现.
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="config">ConventionRegistryConfig</param>
    public void AppendConventionRegistry(Dictionary<string, ConventionPack> dic)
    {
        foreach (var item in dic)
        {
            ConventionRegistry.Add(item.Key, item.Value);
        }
    }
}