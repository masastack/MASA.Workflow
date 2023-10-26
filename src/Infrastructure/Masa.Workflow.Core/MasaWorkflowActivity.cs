namespace Masa.Workflow.Core;

public abstract class MasaWorkflowActivity<TInput> : WorkflowActivity<TInput, ActivityExecutionResult>
    where TInput : ActivityInput
{
    protected readonly Msg _msg;

    public MasaWorkflowActivity(Msg msg)
    {
        _msg = msg;
    }

    public sealed override async Task<ActivityExecutionResult> RunAsync(WorkflowActivityContext context, TInput meta)
    {
        ActivityExecutionResult result = new();
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

    public virtual Task<ActivityExecutionResult> RunAsync(TInput meta)
    {
        var result = new ActivityExecutionResult();
        result.Status = ActivityStatus.Finished;
        result.Wires = meta.Wires;
        result.ActivityId = meta.ActivityId.ToString();
        return Task.FromResult(result);
    }

    public virtual async Task ActivityExecuting(Guid activityId)
    {
        //await _workflowHub.BroadcastStepAsync(new ExecuteStep(activityId, ExecuteStatus.Start));
        await Task.CompletedTask;
    }

    public virtual async Task ActivityExecuted(Guid activityId)
    {
        //await _workflowHub.BroadcastStepAsync(new ExecuteStep(activityId, ExecuteStatus.End));
        await Task.CompletedTask;
    }
}
