namespace Masa.Workflow.Core.Models;

public class ActivityExecutionResult
{
    public ActivityStatus Status { get; set; }

    public string ActivityId { get; set; }

    public List<List<Guid>> Wires { get; set; } = new();

    public List<Message?> Messages { get; set; } = new();
}

public enum ActivityStatus
{
    Running,
    Finished,
    Suspended,
    Faulted,
}