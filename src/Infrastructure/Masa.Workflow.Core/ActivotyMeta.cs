namespace Masa.Workflow.Core;

public abstract class ActivotyMeta
{
    public Guid ActivityId { get; set; } = Guid.Empty;

    public List<List<Guid>> Wires { get; } = new();
}
