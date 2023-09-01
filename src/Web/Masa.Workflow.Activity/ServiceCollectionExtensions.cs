namespace Masa.Workflow.Activity;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDepend(this IServiceCollection services)
    {
        services.AddScoped<Msg>();
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
