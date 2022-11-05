using Microsoft.AspNetCore.Mvc.Filters;

namespace Hoyo.WebCore;
/// <summary>
/// Action过滤器,主要用于统一格式化返回数据结构.
/// </summary>
public class ActionExecuteFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        //暂时不要
        //switch (context.Result)
        //{
        //    case ObjectResult { Value: not null } result when result.Value.GetType().IsSubclassOf(typeof(Stream)):
        //        break;
        //    case ObjectResult result:
        //        context.Result = new ObjectResult(new { StatusCode = HttpStatusCode.OK, Msg = "success", Data = result.Value });
        //        break;
        //    case EmptyResult:
        //        context.Result = new ObjectResult(new { StatusCode = HttpStatusCode.OK, Msg = "success", Data = default(object) });
        //        break;
        //}
        base.OnActionExecuted(context);
    }
}