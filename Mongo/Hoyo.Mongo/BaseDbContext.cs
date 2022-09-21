using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Hoyo.Mongo;
/// <summary>
/// mongodb base context
/// </summary>
public class BaseDbContext
{
    /// <summary>
    /// MongoClient
    /// </summary>
    public IMongoClient _client = default!;
    /// <summary>
    /// 获取链接字符串或者HoyoMongoSettings中配置的特定名称数据库或默认数据库hoyo
    /// </summary>
    public IMongoDatabase _database = default!;

    private static readonly ConventionPackOptions options = new();
    /// <summary>
    ///  使用链接字符串创建客户端,并提供字符串中的数据库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connectionString"></param>
    /// <param name="db"></param>
    /// <returns></returns>
    internal static T CreateInstance<T>(string connectionString, string db = HoyoStatic.HoyoDbName) where T : BaseDbContext
    {
        var t = Activator.CreateInstance<T>();
        var mongoUrl = new MongoUrl(connectionString);
        t._client = new MongoClient(mongoUrl);
        var dbname = !string.IsNullOrWhiteSpace(mongoUrl.DatabaseName) ? mongoUrl.DatabaseName : db;
        t._database = t._client.GetDatabase(dbname);
        return t;
    }

    internal static T CreateInstance<T>(HoyoMongoSettings settings) where T : BaseDbContext
    {
        var t = Activator.CreateInstance<T>();
        if (settings.Validate) throw new("服务器地址或者数据库名为空");
        t._client = new MongoClient(settings.ClientSettings);
        var dbname = !string.IsNullOrWhiteSpace(settings.DatabaseName) ? settings.DatabaseName : HoyoStatic.HoyoDbName;
        t._database = t._client.GetDatabase(dbname);
        return t;
    }

    internal static void RegistryConventionPack(HoyoMongoOptions hoyoOptions)
    {
        hoyoOptions.ConventionPackOptionsAction?.Invoke(options);
        try
        {
            if (hoyoOptions.RegistryPackFirst)
            {
                if (!hoyoOptions.UseDefalutConventionRegistryConfig)
                {
                    _ = hoyoOptions.ConventionRegistry.Remove(HoyoStatic.HoyoPack);
                }
                foreach (var item in hoyoOptions.ConventionRegistry)
                {
                    ConventionRegistry.Register(item.Key, item.Value, _ => true);
                }
                BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));//to local time
                BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));//decimal to decimal default
                                                                                              //BsonSerializer.RegisterSerializer(new TimeOnlySerializer());
            }
        }
        catch (Exception ex)
        {
            throw new($"已注册commonpack,请在第一次调用RegistConventionPack方法后修改 [first] 参数等于 false:{ex.Message}");
        }
        ConventionRegistry.Register($"hoyoidpack", new ConventionPack
        {
            new StringObjectIdIdGeneratorConvention()//Id[string] mapping ObjectId
        }, x => options.IsConvertObjectIdToStringType(x) == false);
    }
}