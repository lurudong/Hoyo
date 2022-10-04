using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Extensions;
using Hoyo.WebCore.Attributes;
using Hoyo.WebCore.SwaggerFilters;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace example.net7.api;

/// <summary>
/// Swagger文档的配置
/// </summary>
public class SwaggerModule : AppModule
{
    /**
     * https://github.com/domaindrivendev/Swashbuckle.AspNetCore
     */
    private const string Title = "example.net7.api";
    private const string Version = "v1";

    /// <summary>
    /// 配置和注册服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _ = context.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(Title, new()
            {
                Title = Title,
                Version = Version,
                Description = "Console.WriteLine(\"🐂🍺\")"
            });
            var controllers = AssemblyHelper.FindTypesByAttribute<ApiGroupAttribute>();
            foreach (var ctrl in controllers)
            {
                var attr = ctrl.GetCustomAttribute<ApiGroupAttribute>();
                if (attr is null) continue;
                c.SwaggerDoc(attr.Title, new()
                {
                    Title = attr.Title,
                    Version = attr.Version,
                    Description = attr.Description
                });
            }
            var files = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var file in files)
            {
                c.IncludeXmlComments(file, true);
            }
            c.DocInclusionPredicate((docName, apiDescription) =>
            {
                //反射拿到值
                var actionList = apiDescription.ActionDescriptor.EndpointMetadata.Where(x => x is ApiGroupAttribute).ToList();
                if (actionList.Any()) {
                    return actionList.FirstOrDefault() is ApiGroupAttribute attr && attr.Title == docName;
                }
                var not = apiDescription.ActionDescriptor.EndpointMetadata.Where(x => x is not ApiGroupAttribute).ToList();
                return not.Any() && docName == Title;
                //判断是否包含这个分组
            });
            c.AddSecurityDefinition("Bearer", new()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });
            // 这里使用预定义的过滤器,避免给所有接口均加锁.
            c.OperationFilter<SwaggerAuthorizeFilter>();
            c.DocumentFilter<SwaggerHiddenApiFilter>();
        });
    }

    /// <summary>
    /// 注册中间件
    /// </summary>
    /// <param name="context"></param>
    public override void ApplicationInitialization(ApplicationContext context)
    {
        var app = context.GetApplicationBuilder();
        _ = app.UseSwagger().UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{Title}/swagger.json", $"{Title} {Version}");
            var controllers = AssemblyHelper.FindTypesByAttribute<ApiGroupAttribute>();
            foreach (var ctrl in controllers)
            {
                var attr = ctrl.GetCustomAttribute<ApiGroupAttribute>();
                if (attr is null) continue;
                c.SwaggerEndpoint($"/swagger/{attr.Title}/swagger.json", $"{attr.Title} {attr.Version}");
            }
        });
    }
}