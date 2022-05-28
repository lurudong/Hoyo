using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.AutoDependencyInjectionModule.Modules;

public class ModulesOptions
{
    public IServiceCollection Service { get; }

    public ModulesOptions(IServiceCollection service) => Service = service;
}