using MongoDB.Driver;

namespace Hoyo.Mongo;
public class HoyoMongoSettings
{
    /// <summary>
    /// 验证数据库
    /// </summary>
    public string AuthDatabase { get; set; } = "admin";
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// 服务地址通常是IP地址或者域名
    /// </summary>
    public List<MongoServerAddress> ServerAddresses { get; set; } = new();
    /// <summary>
    /// 数据库名称
    /// </summary>
    public string DatabaseName { get; set; } = HoyoStatic.HoyoDbName;
    /// <summary>
    /// 获取客户端设置
    /// </summary>
    internal MongoClientSettings ClientSettings
    {
        get => new()
        {
            Credential = MongoCredential.CreateCredential(AuthDatabase, UserName, Password),
            Servers = ServerAddresses
        };
    }
    /// <summary>
    /// 验证地址是否存在或数据库名称不为空
    /// </summary>
    public bool Validate => ServerAddresses.Count == 0;
}
