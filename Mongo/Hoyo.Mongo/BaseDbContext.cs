﻿using MongoDB.Bson.Serialization.Conventions;
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
    public IMongoClient Client { get; private set; } = default!;
    /// <summary>
    /// 获取链接字符串或者HoyoMongoSettings中配置的特定名称数据库或默认数据库hoyo
    /// </summary>
    public IMongoDatabase Database { get; private set; } = default!;

    /// <summary>
    ///  使用链接字符串创建客户端,并提供字符串中的数据库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connStr">链接字符串</param>
    /// <param name="db">数据库名称</param>
    /// <returns></returns>
    internal static T CreateInstance<T>(string connStr, string db = HoyoStatic.HoyoDbName) where T : BaseDbContext
    {
        var t = Activator.CreateInstance<T>();
        var mongoUrl = new MongoUrl(connStr);
        t.Client = new MongoClient(mongoUrl);
        var dbName = !string.IsNullOrWhiteSpace(mongoUrl.DatabaseName) ? mongoUrl.DatabaseName : db;
        t.Database = t.Client.GetDatabase(dbName);
        return t;
    }
    /// <summary>
    /// 使用ClientSettings配置服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="settings"></param>
    /// <returns></returns>
    internal static T CreateInstance<T>(HoyoMongoSettings settings) where T : BaseDbContext
    {
        var t = Activator.CreateInstance<T>();
        if (!settings.Servers.Any()) throw new("服务器地址或者数据库名为空");
        t.Client = new MongoClient(settings.ClientSettings);
        var dbName = !string.IsNullOrWhiteSpace(settings.DatabaseName) ? settings.DatabaseName : HoyoStatic.HoyoDbName;
        t.Database = t.Client.GetDatabase(dbName);
        return t;
    }
    /// <summary>
    /// 注册转换Pack
    /// </summary>
    /// <param name="options"></param>
    internal static void RegistryConventionPack(HoyoMongoOptions options)
    {
        foreach (var item in options.ConventionRegistry)
        {
            ConventionRegistry.Register(item.Key, item.Value, _ => true);
        }
        if (!options.DefaultConventionRegistry)
        {
            ConventionRegistry.Remove(HoyoStatic.HoyoPack);
        }
        ConventionRegistry.Register($"hoyo-id-pack-{Guid.NewGuid()}", new ConventionPack
        {
            new StringObjectIdIdGeneratorConvention()//ObjectId → String mapping ObjectId
        }, x => options.ObjectIdToStringTypes.Contains(x) == false);
    }
}