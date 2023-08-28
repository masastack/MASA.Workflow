using Microsoft.Extensions.DependencyInjection;

namespace Masa.Workflow.RCL;

public static class ServiceCollectionExtensions
{
    public static void AddMasaWorkflowUI(this IServiceCollection services)
    {
        services.AddScoped<DrawflowService>();
        services.AddMasaBlazor(options => { options.Defaults = GlobalConfig.ComponentDefaults; });
    }
}
