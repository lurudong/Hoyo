using MongoDB.Driver.GridFS;

namespace Hoyo.Mongo.GridFS;
public class HoyoGridFSOptions
{
    public GridFSBucketOptions? Options { get; set; } = null;
    public string BusinessApp { get; set; } = string.Empty;
    public bool DefalutDB { get; set; } = true;
    public string ItemInfo { get; set; } = "item.info";
}