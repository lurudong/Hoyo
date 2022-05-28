namespace Hoyo.Mongo.GridFS.Extension;
public class HoyoStaticFileSettings
{
    public const string Postion = "HoyoStaticFile";
    public string VirtualPath { get; set; } = string.Empty;
    public string PhysicalPath { get; set; } = string.Empty;
}