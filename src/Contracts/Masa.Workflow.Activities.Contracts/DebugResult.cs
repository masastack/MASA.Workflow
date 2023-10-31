namespace Masa.Workflow.Activities.Contracts;

public record DebugResult
{
    public DateTimeOffset DebugAt { get; set; }

    public string Log { get; set; } = string.Empty;
}
