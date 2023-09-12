namespace Masa.Workflow.Activities.Contracts;

public enum WorkflowStatus
{
    Idle,
    Running,
    Finished,
    Suspended,
    Faulted,
    Cancelled
}
