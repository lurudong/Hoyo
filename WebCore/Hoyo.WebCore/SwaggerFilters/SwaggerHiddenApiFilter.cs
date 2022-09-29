using Hoyo.WebCore.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Hoyo.WebCore.SwaggerFilters;

/// <summary>
/// 在Swagger文档中隐藏接口
/// </summary>
public class SwaggerHiddenApiFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var apiDescription in context.ApiDescriptions)
        {
            if (!apiDescription.TryGetMethodInfo(out var method) || (!method.ReflectedType!.IsDefined(typeof(HiddenApiAttribute)) && !method.IsDefined(typeof(HiddenApiAttribute))))
                continue;
            var key = $"/{apiDescription.RelativePath}";
            if (key.Contains('?'))
            {
                var index = key.IndexOf("?", StringComparison.Ordinal);
                key = key[..index];
            }
            _ = swaggerDoc.Paths.Remove(key);
        }
    }
}