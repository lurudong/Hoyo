using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Hoyo.Mongo;
/// <summary>
/// mongodb base context
/// </summary>
public class BaseDbContext : IDbSet
{
    public IMongoClient? _client;
    public IMongoDatabase? _database;

    private static readonly ConventionPackOptions options = new();

    public static T CreateInstance<T>(string connectionString, string db = "") where T : BaseDbContext
    {
        var t = Activator.CreateInstance<T>();
        if (string.IsNullOrWhiteSpace(connectionString)) throw new("连接字符串为空");
        var mongoUrl = new MongoUrl(connectionString);
        t._client = new MongoClient(mongoUrl);
        var dbname = string.IsNullOrWhiteSpace(db) ? mongoUrl.DatabaseName : db;
        t._database = t._client.GetDatabase(dbname);        
        return t;
    }

    public static T CreateInstance<T>(HoyoMongoClientSettings clientSettings) where T : BaseDbContext
    {
        var t = Activator.CreateInstance<T>();
        if (clientSettings.Validate) throw new("服务器地址或者数据库名为空");
        t._client = new MongoClient(clientSettings.ClientSettings);
        var dbname = !string.IsNullOrWhiteSpace(clientSettings.DatabaseName) ? clientSettings.DatabaseName : "hoyo";
        t._database = t._client.GetDatabase(dbname);
        return t;
    }

    public static void RegistryConventionPack(HoyoMongoOptions hoyoOptions)
    {
        hoyoOptions.ConventionPackOptionsAction?.Invoke(options);
        if (hoyoOptions.First is not null & hoyoOptions.First is true)
        {
            try
            {
                foreach (var item in hoyoOptions.ConventionRegistry)
                {
                    ConventionRegistry.Register(item.Key, item.Value.Conventions, item.Value.Filter);
                }
                BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));//to local time
                BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));//decimal to decimal default
                //BsonSerializer.RegisterSerializer(new TimeOnlySerializer());
                //BsonSerializer.RegisterSerializer(new DateOnlySerializer());
            }
            catch (Exception ex)
            {
                throw new($"已注册commonpack,请在第一次调用RegistConventionPack方法后修改 [first] 参数等于 false:{ex.Message}");
            }
        }
        ConventionRegistry.Register($"idpack{Guid.NewGuid()}", new ConventionPack
        {
            new StringObjectIdIdGeneratorConvention()//Id[string] mapping ObjectId
        }, x => options.IsConvertObjectIdToStringType(x) == false);
    }

    protected virtual string[] GetTransactColletions() => Array.Empty<string>();

    public void BuildTransactCollections()
    {
        if (_database is null) throw new("_database 还未准备好,请在使用该函数前初始化DbContext");
        var transcolls = GetTransactColletions();
        if (transcolls.Length <= 0) return;
        var count = 1;
        while (CreateCollections(transcolls).Result == false && count < 10)
        {
            Console.WriteLine($"[🤪]BuildTransactCollections:{count} 次错误,将在下一秒重试.[{DateTime.Now.ToLongTimeString()}]");
            count++;
            Thread.Sleep(1000);
        }
    }

    private async Task<bool> CreateCollections(IEnumerable<string> collections)
    {
        if (_database is null) throw new("_database 还未准备好,请在使用该函数前初始化DbContext");
        try
        {
            var exists = (await _database?.ListCollectionNamesAsync()!).ToList();
            var unexists = collections.Where(x => exists?.Exists(c => c == x) == false);
            foreach (var collection in unexists) _ = _database?.CreateCollectionAsync(collection)!;
            Console.WriteLine("[🎉]CreateCollections:创建集合成功");
            return true;
        }
        catch
        {
            return false;
        }
    }
}