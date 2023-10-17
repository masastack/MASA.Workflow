namespace Masa.Workflow.Activities;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasaWorkflow(this IServiceCollection services)
    {
        services.AddMasaWorkflowCore(Assembly.GetExecutingAssembly());
        services.AddRulesEngine(rulesEngineOptions =>
        {
            rulesEngineOptions.UseMicrosoftRulesEngine(new RulesEngine.Models.ReSettings
            {
                CustomTypes = new[] { typeof(Regex), typeof(RegexOptions), typeof(Ruler) }
            });
        });
        return services;
    }
}