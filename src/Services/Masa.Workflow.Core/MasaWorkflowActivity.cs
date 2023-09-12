namespace Masa.Workflow.Core;

public abstract class MasaWorkflowActivity<TMeta> : WorkflowActivity<TMeta, List<List<Guid>>> where TMeta : MetaBase
{
    protected readonly Msg _msg;

    public MasaWorkflowActivity(Msg msg)
    {
        _msg = msg;
    }

    public sealed override async Task<List<List<Guid>>> RunAsync(WorkflowActivityContext context, TMeta meta)
    {
        List<List<Guid>> result = new();
        await ActivityExecuting(meta.ActivityId);
        try
        {
            result = await RunAsync(meta);
        }
        catch (Exception)
        {
            //await _workflowHub.BroadcastStepAsync(new ExecuteStep(meta.ActivityId, ExecuteStatus.Fail));
        }
        await ActivityExecuted(meta.ActivityId);
        return result;
    }

    public virtual Task<List<List<Guid>>> RunAsync(TMeta meta)
    {
        return Task.FromResult(meta.Wires);
    }

    public virtual async Task ActivityExecuting(Guid activityId)
    {
        //await _workflowHub.BroadcastStepAsync(new ExecuteStep(activityId, ExecuteStatus.Start));
    }

    public virtual async Task ActivityExecuted(Guid activityId)
    {
        //await _workflowHub.BroadcastStepAsync(new ExecuteStep(activityId, ExecuteStatus.End));
    }
}
