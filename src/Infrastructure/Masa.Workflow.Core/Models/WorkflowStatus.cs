namespace Masa.Workflow.Core.Models;

public enum WorkflowStatus
{
    Idle,
    Running,
    Finished,
    Suspended,
    Faulted,
    Cancelled
}
