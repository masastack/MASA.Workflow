namespace Masa.Workflow.Activities.Contracts.Switch;

public class SwitchMeta
{
    public string Property { get; set; } = "Payload";

    public List<Rule> Rules { get; set; } = new();

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

    public int HashCode => GetHashCode();
}
