namespace Masa.Workflow.Activity.Activities.Switch;

public class SwitchMeta
{
    public string Property { get; set; } = "Payload";

    public List<KeyValuePair<Operator, object?>> Rules = new();

    public List<List<Guid>> Wires { get; set; } = new();

    public EnforceRule EnforceRule { get; set; } = EnforceRule.All;
}
