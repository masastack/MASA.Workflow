namespace Masa.Workflow.Core;

public class WorkflowHub : Hub
{
    public async Task BroadcastStepAsync(ExecuteStep step)
    {
        await Clients.All.SendAsync("ExecuteStep", step);
    }
}

public record ExecuteStep(Guid ActivityId, ExecuteStatus Status);


public enum ExecuteStatus
{
    Start,
    End,
    Fail
}