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

    public MetaData Meta { get; private set; }

    /// <summary>
    /// next node id collection
    /// </summary>
    public Wires Wires { get; private set; }

    public Activity(Guid id, string name, string type, string? description) : this(id, name, type, description, null, null)
    {
    }

    public Activity(Guid id, string name, string type, string? description, Wires? wires, MetaData? meta)
    {
        Id = id;
        Name = name;
        Description = description ?? "";
        Type = type;
        Wires = wires ?? new();
        Meta = meta ?? new();
    }

    public void Update(string name, string? description, Wires? wires, MetaData? meta)
    {
        if (!name.IsNullOrEmpty())
        {
            Name = name;
        }

        if (!description.IsNullOrEmpty())
        {
            Description = description;
        }

        if (wires != null)
        {
            Wires = wires;
        }

        if (meta != null)
        {
            Meta = meta;
        }
    }
}

public class MetaData : Dictionary<string, object>
{

}

public class Wires : List<List<Guid>>
{

}