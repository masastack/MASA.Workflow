namespace Masa.Workflow.Activities.Contracts.Debug;

public class DebugMeta
{
    public string? Property { get; set; }

    public bool LogToDebugWindow { get; set; }

    public DebugMeta()
    {
        LogToDebugWindow = true;
        Property = "Payload";
    }
}
