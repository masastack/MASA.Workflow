namespace Masa.Workflow.Activity.Activities.Complete;

public class CompleteActivity : MasaWorkflowActivity<SwitchMeta>
{
    public CompleteActivity(WorkflowHub workflowHub, Msg msg) : base(workflowHub, msg)
    {
    }

    public override Task<List<List<Guid>>> RunAsync(SwitchMeta meta)
    {

        throw new NotImplementedException();
    }
}
