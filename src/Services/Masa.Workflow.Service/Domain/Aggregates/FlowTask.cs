namespace Masa.Workflow.Service.Domain.Aggregates;

public class FlowTask : FullEntity<Guid, Guid>
{
    public DateTimeOffset RunTime { get; private set; }

    public DateTimeOffset StartTime { get; private set; } = DateTimeOffset.MinValue;

    public DateTimeOffset EndTime { get; private set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// Task run use total time (second)
    /// </summary>
    public long RunTimeSpan { get; private set; }

    public string WorkerHost { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

}
