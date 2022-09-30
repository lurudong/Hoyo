using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Hoyo.WebCore;
/// <summary>
/// 全局异常过滤器
/// </summary>
public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionFilter> _logger;
    public ExceptionFilter(ILogger<ExceptionFilter> logger) => _logger = logger;
    public override Task OnExceptionAsync(ExceptionContext context)
    {
        _logger.LogError("{stacktrace}", context.Exception.ToString());
        context.Result = new ObjectResult(new
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Msg = context.Exception.Message,
            Data = default(object)
        });
        context.ExceptionHandled = true;
        return base.OnExceptionAsync(context);
    }
}