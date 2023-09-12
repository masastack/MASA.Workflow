namespace Masa.Workflow.Activities;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasaWorkflow(this IServiceCollection services)
    {
        services.AddScoped<Msg>();
        services.AddDaprWorkflow(options =>
        {
            options.RegisterWorkflow<MasaWorkFlow>();

            var assembly = Assembly.GetExecutingAssembly();
            var activityTypes = assembly.GetTypes()
                .Where(type => type.BaseType != null
                    && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(MasaWorkflowActivity<>));

            var registerActivityMethod = options.GetType().GetMethod("RegisterActivity", Type.EmptyTypes);
            if (registerActivityMethod == null)
            {
                throw new Exception("Unable to find 'RegisterActivity' method.");
            }
            foreach (var activityType in activityTypes)
            {
                var genericMethod = registerActivityMethod.MakeGenericMethod(activityType);
                genericMethod.Invoke(options, null);
            }
        });

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