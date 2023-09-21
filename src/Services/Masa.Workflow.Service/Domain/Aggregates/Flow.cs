namespace Masa.Workflow.Service.Domain.Aggregates;

public class Flow : FullAggregateRoot<Guid, Guid>
{
    private List<FlowVersion> _versions = new();
    private List<Activity> _activities = new();

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool Disabled { get; private set; }

    public bool IsDraft { get; private set; }

    public IReadOnlyCollection<FlowVersion> Versions => _versions;

    public IReadOnlyCollection<Activity> Activities => _activities;

    public Dictionary<string, object> EnvironmentVariables { get; private set; } = new();

    public Flow(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
