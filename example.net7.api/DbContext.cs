using Hoyo.Mongo;
using MongoDB.Driver;

namespace example.net7.api;
public class DbContext : BaseDbContext
{
    /// <summary>
    /// 作息时间管理
    /// </summary>
    public IMongoCollection<MongoTest> Test => Database.GetCollection<MongoTest>("mongo.test");
}

public class DbContext2 : BaseDbContext
{
    /// <summary>
    /// 作息时间管理
    /// </summary>
    public IMongoCollection<MongoTest> Test => Database.GetCollection<MongoTest>("mongo.test2");
}
public class MongoTest
{
    public string Id { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public TimeSpan TimeSpan { get; set; }
    public DateOnly DateOnly { get; set; }
    public TimeOnly TimeOnly { get; set; }
}