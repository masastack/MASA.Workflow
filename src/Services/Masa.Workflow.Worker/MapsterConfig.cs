namespace Masa.Workflow.Worker;

public sealed class MapsterConfig
{
    public static void Map()
    {
        TypeAdapterConfig<WorkflowDefinition, WorkflowInstance>
            .NewConfig()
            .Map(dest => dest.Activities, src => src.Nodes);

        TypeAdapterConfig<RetryPolicy, Core.Models.RetryPolicy>
            .NewConfig()
            .MapWith(src => new Core.Models.RetryPolicy()
            {
                MaxNumberOfAttempts = src.MaxNmberOfAttempts,
                BackoffCoefficient = src.BackoffCoefficient,
                FirstRetryInterval = src.FirstRetryInterval.ToTimeSpan(),
                MaxRetryInterval = src.MaxRetryInterval.ToTimeSpan(),
                RetryTimeout = src.RetryTimeout.ToTimeSpan()
            });
    }
}
