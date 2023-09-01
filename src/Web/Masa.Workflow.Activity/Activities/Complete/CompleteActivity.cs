namespace Masa.Workflow.Activity.Activities.Complete;

public class CompleteActivity : MasaWorkflowActivity<SwitchMeta>
{
    public CompleteActivity(WorkflowHub workflowHub, Msg msg) : base(workflowHub, msg)
    {
    }
}
