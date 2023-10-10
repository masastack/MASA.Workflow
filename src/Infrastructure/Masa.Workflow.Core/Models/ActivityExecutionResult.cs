namespace Masa.Workflow.Core.Models;

public class ActivityExecutionResult
{
    public ActivityStatus Status { get; set; }

    public string ActivityId { get; set; }

    public Wires Wires { get; set; } = new Wires();
}

public enum ActivityStatus
{
    Running,
    Finished,
    Suspended,
    Faulted,
}