using Masa.Workflow.Activities.Contracts.Switch;

namespace Masa.Workflow.Activity.Activities.Switch;

public class SwitchMeta : MetaBase
{
    public string Property { get; set; } = "Payload";

    public List<Rule> Rules { get; set; } = new();

    public List<List<Guid>> Wires { get; set; } = new();

    public SwitchMode SwitchMode { get; set; } = SwitchMode.All;
}
public class Rule
{
    public Rule()
    {
    }

    public Rule(Operator @operator) : this(@operator, null)
    {
    }

    public Rule(Operator @operator, string? value)
    {
        Operator = @operator;
        Value = value;
    }

    public Operator Operator { get; set; }

    public string? Value { get; set; }

    public int HashCode => this.GetHashCode();
}
