namespace Masa.Workflow.Activities.Contracts;

public abstract class MetaBase
{
    public Guid ActivityId { get; } = Guid.NewGuid();

    public List<List<Guid>> Wires { get; } = new();
}
