namespace Masa.Workflow.Service.Domain.Aggregates;

public class Activity : Entity<Guid>
{
    public Guid FlowId { get; init; }

    public Flow Flow { get; private set; } = default!;

    public string Type { get; init; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool Disabled { get; private set; }

    public RetryPolicyValue RetryPolicy { get; private set; } = new();

    public FlowVersion Version { get; private set; } = default!;

    public MetaData Meta { get; private set; } = new();

    /// <summary>
    /// next node id collection
    /// </summary>
    public Wires Wires { get; private set; } = new();

    public Activity(string name, string type, string description = "")
    {
        Name = name;
        Description = description;
        Type = type;
    }
}

public class MetaData : Dictionary<string, object>
{

}

public class Wires : List<List<Guid>>
{

}