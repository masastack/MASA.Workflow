namespace Masa.Workflow.Activity.Activities.Switch;

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
