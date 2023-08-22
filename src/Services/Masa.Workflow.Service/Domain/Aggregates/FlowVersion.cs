namespace Masa.Workflow.Service.Domain.Aggregates;

public class FlowVersion : FullEntity<Guid, Guid>
{
    public Guid FlowId { get; init; }

    public Flow Flow { get; private set; } = default!;

    public bool IsDraft { get; private set; }

    public string VersionNumber { get; init; }

    public List<Activity> Activities { get; init; }

    private FlowVersion()
    {
        VersionNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
        Activities = new();
    }

    public FlowVersion(bool isDraft, List<Activity> activities) : this()
    {
        IsDraft = isDraft;
        Activities = activities;
    }

    public void Release()
    {
        IsDraft = false;
    }
}
