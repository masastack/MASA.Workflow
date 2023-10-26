namespace Masa.Workflow.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasaWorkflowCore(this IServiceCollection services, Assembly assembly)
    {
        services.AddDaprWorkflow(options =>
        {
            options.RegisterWorkflow<MasaWorkFlow>();
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

        TypeAdapterConfig<RetryPolicy, WorkflowRetryPolicy>.NewConfig()
                                                  .MapWith(src => new WorkflowRetryPolicy(src.MaxNumberOfAttempts, src.FirstRetryInterval,
                                                  src.BackoffCoefficient, src.MaxRetryInterval, src.RetryTimeout));

        return services;
    }
}