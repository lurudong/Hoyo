using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Hoyo.Mongo.GridFS.Extension;
public static class GridFSVirtualPathExtension
{
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
