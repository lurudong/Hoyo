using Hoyo.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hoyo.EventBus.RabbitMQ;
public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseEventBusRabbitMQ(this IApplicationBuilder builder)
    {
        var eventBus = builder.ApplicationServices.GetService<IIntegrationEventBus>();
        if (eventBus is null)
        {
            throw new Exception("RabbitMQ集成事件总线没有注入");
        }
        var handlerTypes = AssemblyHelper.FindTypes(o => o.IsClass && !o.IsAbstract && o.IsBaseOn(typeof(IIntegrationEventHandler<>)));
        foreach (var handlerType in handlerTypes)
        {
            var implementedType = handlerType.GetTypeInfo().ImplementedInterfaces.Where(o => o.IsBaseOn(typeof(IIntegrationEventHandler<>))).FirstOrDefault();
            var eventType = implementedType?.GetTypeInfo().GenericTypeArguments.FirstOrDefault();
            if (eventType is null)
            {
                continue;
            }
            eventBus.Subscribe(eventType, handlerType);
        }
        return builder;
    }
}