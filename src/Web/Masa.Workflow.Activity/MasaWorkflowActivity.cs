namespace Masa.Workflow.Activity;

public abstract class MasaWorkflowActivity<TMeta, TOut> : WorkflowActivity<Msg<TMeta>, TOut?> where TMeta : new()
{
    //todo TMeta 修改为全局配置读取
    readonly WorkflowHub _workflowHub;

    public MasaWorkflowActivity(WorkflowHub workflowHub)
    {
        _workflowHub = workflowHub;
    }

    public sealed override async Task<TOut?> RunAsync(WorkflowActivityContext context, Msg<TMeta> msg)
    {
        TOut? result = default;
        await ActivityExecuting(msg.ActivityId);
        try
        {
            result = await RunAsync(msg);
        }
        catch (Exception)
        {
            await _workflowHub.BroadcastStepAsync(new ExecuteStep(msg.ActivityId, ExecuteStatus.Fail));
        }
        await ActivityExecuted(msg.ActivityId);
        return result;
    }

    public abstract Task<TOut> RunAsync(Msg<TMeta> msg);

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
