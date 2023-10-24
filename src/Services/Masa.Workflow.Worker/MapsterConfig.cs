namespace Masa.Workflow.Worker;

public sealed class MapsterConfig
{
    public static void Map()
    {
        TypeAdapterConfig<WorkflowDefinition, WorkflowInstance>
            .NewConfig()
            .Map(dest => dest.Activities, src => src.Activities);

        TypeAdapterConfig<RetryPolicy, Core.Models.RetryPolicy>
            .NewConfig()
            .MapWith(src => new Core.Models.RetryPolicy()
            {
                MaxNumberOfAttempts = src.MaxNumberOfAttempts, 
                BackoffCoefficient = src.BackoffCoefficient,
                FirstRetryInterval = src.FirstRetryInterval,
                MaxRetryInterval = src.MaxRetryInterval,
                RetryTimeout = src.RetryTimeout
            });
    }
}
