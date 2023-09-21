namespace Masa.Workflow.Activity.Functions.Switch;

public class SwitchActivityMeta : ActivityMeta<SwitchMeta>
{
    public SwitchActivityMeta()
    {
        if (MetaData.Rules.Count == 0)
        {
            MetaData.Rules.Add(new Rule(Operator.Eq, null));
        }
    }
}
