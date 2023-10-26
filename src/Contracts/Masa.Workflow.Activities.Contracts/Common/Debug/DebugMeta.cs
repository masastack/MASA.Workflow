namespace Masa.Workflow.Activities.Contracts.Debug;

public class DebugMeta
{
    public Guid[]? DependentIds { get; set; }

    public bool DebugToConsole { get; set; }
}
