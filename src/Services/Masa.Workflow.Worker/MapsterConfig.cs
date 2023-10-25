namespace Masa.Workflow.Worker;

public sealed class MapsterConfig
{
    public static void Map()
    {
        TypeAdapterConfig<WorkflowDefinition, WorkflowInstance>
            .NewConfig()
            .Map(dest => dest.Activities, src => src.Activities);

        TypeAdapterConfig<Activity.Types.RetryPolicy, RetryPolicy>
            .NewConfig()
            .MapWith(src => new RetryPolicy()
            {
                MaxNumberOfAttempts = src.MaxNumberOfAttempts,
                BackoffCoefficient = src.BackoffCoefficient,
                FirstRetryInterval = src.FirstRetryInterval.ToTimeSpan(),
                MaxRetryInterval = src.MaxRetryInterval.ToTimeSpan(),
                RetryTimeout = src.RetryTimeout.ToTimeSpan()
            });
    }
}
