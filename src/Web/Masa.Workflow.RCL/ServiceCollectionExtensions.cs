using Microsoft.Extensions.DependencyInjection;

namespace Masa.Workflow.RCL;

public static class ServiceCollectionExtensions
{
    public static void AddMasaWorkflowUI(this IServiceCollection services)
    {
        services.AddMasaBlazor(options => { options.Defaults = GlobalConfig.ComponentDefaults; });
    }
}
