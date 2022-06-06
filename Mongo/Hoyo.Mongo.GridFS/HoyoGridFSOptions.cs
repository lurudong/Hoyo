using MongoDB.Driver.GridFS;

namespace Hoyo.Mongo.GridFS;
public class HoyoGridFSOptions
{
    /// <summary>
    /// GridFSBucketOptions
    /// </summary>
    public GridFSBucketOptions? Options { get; set; } = null;
    /// <summary>
    /// APP名称[通常指业务系统的系统名称]
    /// </summary>
    public string BusinessApp { get; set; } = string.Empty;
    /// <summary>
    /// 默认数据库
    /// </summary>
    public bool DefalutDB { get; set; } = true;
    /// <summary>
    /// 文件信息表名称,默认为[item.info]
    /// </summary>
    public string ItemInfo { get; set; } = "item.info";
}