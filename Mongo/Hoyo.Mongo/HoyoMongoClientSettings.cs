using MongoDB.Driver;

namespace Hoyo.Mongo;
public class HoyoMongoSettings
{
    /// <summary>
    /// 验证数据库
    /// </summary>
    public string AuthDatabase { get; set; } = HoyoStatic.HoyoAuthDataBase;
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// 服务地址通常是IP地址或者域名,默认IP:localhost,Port:27017
    /// </summary>
    public List<MongoServerAddress> Servers { get; set; } = new()
    {
        new(HoyoStatic.HoyoIp, HoyoStatic.HoyoPort)
    };
    /// <summary>
    /// 数据库名称
    /// </summary>
    public string DatabaseName { get; set; } = HoyoStatic.HoyoDbName;
    /// <summary>
    /// 获取客户端设置
    /// </summary>
    internal MongoClientSettings ClientSettings => new()
    {
        Credential = MongoCredential.CreateCredential(AuthDatabase, UserName, Password),
        Servers = Servers
    };
}
