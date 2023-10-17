namespace Masa.Workflow.Core;

public abstract class ActivotyMeta
{
    public Guid ActivityId { get; } = Guid.Empty;

    public List<List<Guid>> Wires { get; } = new();
}
