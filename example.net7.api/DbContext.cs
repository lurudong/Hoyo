using Hoyo.Mongo;
using MongoDB.Driver;

namespace example.net7.api;
/// <summary>
/// DBContext
/// </summary>
public class DbContext : BaseDbContext
{
    /// <summary>
    /// 作息时间管理
    /// </summary>
    public IMongoCollection<MongoTest> Test => Database.GetCollection<MongoTest>("mongo.test");
}
/// <summary>
/// DBContext2
/// </summary>
public class DbContext2 : BaseDbContext
{
    /// <summary>
    /// 作息时间管理
    /// </summary>
    public IMongoCollection<MongoTest> Test => Database.GetCollection<MongoTest>("mongo.test2");
}
/// <summary>
/// Mongo测试数据类型
/// </summary>
public class MongoTest
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// 完整DateTime
    /// </summary>
    public DateTime DateTime { get; set; }
    /// <summary>
    /// TimeSpan类型
    /// </summary>
    public TimeSpan TimeSpan { get; set; }
    /// <summary>
    /// DateOnly类型
    /// </summary>
    public DateOnly DateOnly { get; set; }
    /// <summary>
    /// TimeOnly类型
    /// </summary>
    public TimeOnly TimeOnly { get; set; }
}