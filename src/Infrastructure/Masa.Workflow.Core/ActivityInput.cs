namespace Masa.Workflow.Core;

public class ActivityInput
{
    public Guid ActivityId { get; set; } = Guid.Empty;

    public List<List<Guid>> Wires { get; set; } = new();

    public Msg Msg { get; set; } = new();
}

public class ActivityInput<TMeta> : ActivityInput
    where TMeta : class, new()
{
    public TMeta Meta { get; set; } = new();
}
