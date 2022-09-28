using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Hoyo.WebCore;
/// <summary>
/// API耗时监控中间件,应尽量靠前,越靠前越能体现整个管道中所有管道的耗时,越靠后越能体现Action的执行时间.可根据实际情况灵活配置位置.
/// </summary>
public class ResponseTimeMiddleware
{
    private const string ResponseTime = "Hoyo-Response-Time";
    private readonly RequestDelegate _next;
    public ResponseTimeMiddleware(RequestDelegate next) => _next = next;
    public async Task Invoke(HttpContext context)
    {
        var watch = new Stopwatch();
        watch.Start();
        context.Response.OnStarting(() =>
        {
            watch.Stop();
            context.Response.Headers[ResponseTime] = $"{watch.ElapsedMilliseconds} ms";
            return Task.CompletedTask;
        });
        await _next(context);
    }
}
/// <summary>
/// 全局API耗时监控中间件
/// </summary>
public static class ResponseTimeMiddlewareExtensions
{
    public static IApplicationBuilder UseHoyoResponseTime(this IApplicationBuilder builder) => builder.UseMiddleware<ResponseTimeMiddleware>();
}