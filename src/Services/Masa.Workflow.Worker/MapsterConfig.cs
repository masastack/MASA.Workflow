namespace Masa.Workflow.Worker;

public sealed class MapsterConfig
{
    public static void Map()
    {
        TypeAdapterConfig<WorkflowDefinition, WorkflowInstance>
            .NewConfig()
            .Map(dest => dest.Activities, src => src.Nodes);
    }
}
