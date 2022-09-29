using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Hoyo.Mongo.GridFS.Extension;
/// <summary>
/// 配置虚拟文件路径扩展
/// </summary>
public static class GridFSVirtualPathExtension
{
    /// <summary>
    /// 注册虚拟文件路径中间件.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseHoyoGridFSVirtualPath(this IApplicationBuilder app, IConfiguration config)
    {
        var hoyoFile = config.GetSection(HoyoStaticFileSettings.Position).Get<HoyoStaticFileSettings>();
        if (!Directory.Exists(hoyoFile.PhysicalPath))
        {
            _ = Directory.CreateDirectory(hoyoFile.PhysicalPath);
        }
        _ = app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(hoyoFile.PhysicalPath),
            RequestPath = hoyoFile.VirtualPath
        });
        return app;
    }
}
