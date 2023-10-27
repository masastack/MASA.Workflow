namespace Masa.Workflow.Core;

public class ActivityInfo
{
    public string Input { get; set; }

    public Message Msg { get; set; } = new();

    public Guid ActivityId { get; set; } = Guid.Empty;

    public List<List<Guid>> Wires { get; set; } = new();
}