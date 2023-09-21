namespace Masa.Workflow.Service.Domain.Aggregates;

public class FlowVersion : FullEntity<Guid, Guid>
{
    public Guid FlowId { get; init; }

    public Flow Flow { get; private set; } = default!;

    public string VersionNumber { get; init; }

    public string Json { get; init; }

    private FlowVersion(string json)
    {
        VersionNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
        Json = json;
    }
}
