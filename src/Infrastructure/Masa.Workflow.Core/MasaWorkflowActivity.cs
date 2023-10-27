namespace Masa.Workflow.Core;

public abstract class MasaWorkflowActivity<TInput> : WorkflowActivity<ActivityInfo, ActivityExecutionResult>
{
    public Guid ActivityId { get; private set; }

    public List<List<Guid>> Wires { get; private set; }

    // TODO: comment that do not use this method
    public sealed override async Task<ActivityExecutionResult> RunAsync(WorkflowActivityContext context, ActivityInfo activityInfo)
    {
        ActivityId = activityInfo.ActivityId;
        Wires = activityInfo.Wires;
        var inputObj = Convert(activityInfo.Input);
        ActivityExecutionResult result = new();
        await ActivityExecuting(ActivityId);
        try
        {
            result = await RunAsync(inputObj, activityInfo.Msg);
        }
        catch (Exception)
        {
            //await _workflowHub.BroadcastStepAsync(new ExecuteStep(meta.ActivityId, ExecuteStatus.Fail));
        }
        await ActivityExecuted(ActivityId);
        return result;
    }

    TInput Convert(string input)
    {
        return JsonSerializer.Deserialize<TInput>(input) ?? throw new Exception($"input deserialize {typeof(TInput)} error");
    }

    public virtual Task<ActivityExecutionResult> RunAsync(TInput input, Message msg)
    {
        var result = new ActivityExecutionResult();
        result.Status = ActivityStatus.Finished;
        result.Wires = Wires;
        result.ActivityId = ActivityId.ToString();
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
