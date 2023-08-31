namespace Masa.Workflow.ActivityCore;

public abstract class MasaWorkflowActivity<TMeta> : WorkflowActivity<TMeta, List<List<Guid>>> where TMeta : MetaBase
{
    readonly WorkflowHub _workflowHub;
    protected readonly Msg _msg;

    public MasaWorkflowActivity(WorkflowHub workflowHub, Msg msg)
    {
        _workflowHub = workflowHub;
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
            await _workflowHub.BroadcastStepAsync(new ExecuteStep(meta.ActivityId, ExecuteStatus.Fail));
        }
        await ActivityExecuted(meta.ActivityId);
        return result;
    }

    public abstract Task<List<List<Guid>>> RunAsync(TMeta meta);

    public virtual async Task ActivityExecuting(Guid activityId)
    {
        await _workflowHub.BroadcastStepAsync(new ExecuteStep(activityId, ExecuteStatus.Start));
    }

    public virtual async Task ActivityExecuted(Guid activityId)
    {
        await _workflowHub.BroadcastStepAsync(new ExecuteStep(activityId, ExecuteStatus.End));
    }

    #region UI Style

    public virtual string Icon { get; private set; } = string.Empty;

    public virtual bool ShowLabel { get; private set; }

    public virtual List<string> InputLabels { get; private set; } = new();

    public virtual List<string> OutputLabels { get; private set; } = new();

    public virtual int Output { get; set; } = 1;

    public virtual int Input { get; set; } = 1;

    #endregion
}
