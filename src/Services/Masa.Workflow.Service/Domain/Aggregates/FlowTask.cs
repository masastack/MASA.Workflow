namespace Masa.Workflow.Service.Domain.Aggregates;

public class FlowTask : FullEntity<Guid, Guid>
{
    public DateTimeOffset StartTime { get; private set; }

    public DateTimeOffset EndTime { get; private set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// Task run use total time (second)
    /// </summary>
    public long RunTime { get; private set; } = 0;

    public string WorkerHost { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public WorkflowStatus Status { get; private set; }

    private FlowTask()
    {

    }

    public FlowTask(WorkflowStatus workflowStatus, string workHost)
    {
        Status = workflowStatus;
        WorkerHost = workHost;
        StartTime = DateTimeOffset.UtcNow;
    }

    public void UpdateStatus(WorkflowStatus status)
    {
        Status = status;
        if (status == WorkflowStatus.Finished || status == WorkflowStatus.Cancelled || status == WorkflowStatus.Faulted)
        {
            EndTime = DateTimeOffset.UtcNow;
        }
    }
}
