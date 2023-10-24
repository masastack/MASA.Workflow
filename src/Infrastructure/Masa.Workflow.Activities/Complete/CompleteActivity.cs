using Masa.Workflow.Activities.Console;

namespace Masa.Workflow.Activities.Complete;

public class CompleteActivity : MasaWorkflowActivity<Console.Console>
{
    public CompleteActivity(Msg msg) : base(msg)
    {
    }
}
