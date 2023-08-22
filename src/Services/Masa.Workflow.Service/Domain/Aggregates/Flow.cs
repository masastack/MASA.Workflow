namespace Masa.Workflow.Service.Domain.Aggregates;

public class Flow : FullAggregateRoot<Guid, Guid>
{
    private List<Activity> _activities = new();
    private List<FlowVersion> _versions = new();

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool Disabled { get; private set; }

    public IReadOnlyCollection<Activity> Activities => _activities;

    public IReadOnlyCollection<FlowVersion> Versions => _versions;

    public Dictionary<string, object> EnvironmentVariables { get; private set; } = new();

    public Flow(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void AddVersion(FlowVersion flowVersion)
    {
        if (flowVersion.IsDraft && _versions.Any(v => v.IsDraft))
        {
            throw new UserFriendlyException("already draft");
        }
        _versions.Add(flowVersion);
    }

    public void Deployment()
    {
        var draftVersion = _versions.FirstOrDefault(v => v.IsDraft);
        if (draftVersion != null)
        {
            draftVersion.Release();
        }
    }
}
