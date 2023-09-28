namespace Masa.Workflow.Core.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum WorkflowStatus
{
    Idle,
    Running,
    Finished,
    Suspended,
    Faulted,
    Cancelled
}
