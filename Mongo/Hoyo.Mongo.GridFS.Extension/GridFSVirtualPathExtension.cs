using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Hoyo.Mongo.GridFS.Extension;
public static class GridFSVirtualPathExtension
{
    public static IApplicationBuilder UseHoyoGridFSVirtualPath(this IApplicationBuilder app, IConfiguration config)
    {
        var hoyofile = config.GetSection(HoyoStaticFileSettings.Postion).Get<HoyoStaticFileSettings>();
        if (!Directory.Exists(hoyofile.PhysicalPath))
        {
            _ = Directory.CreateDirectory(hoyofile.PhysicalPath);
        }
        _ = app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(hoyofile.PhysicalPath),
            RequestPath = hoyofile.VirtualPath
        });
        return app;
    }
}
