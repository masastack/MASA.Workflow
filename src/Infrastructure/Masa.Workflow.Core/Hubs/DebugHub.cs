using Masa.Workflow.Activities.Contracts;

namespace Masa.Workflow.Core.Hubs;

public class DebugHub : Hub<IDebugHub>
{
    public override Task OnConnectedAsync()
    {
        if (TryGetWorkflowId(out var workflowId))
        {
            Groups.AddToGroupAsync(Context.ConnectionId, workflowId);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (TryGetWorkflowId(out var workflowId))
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, workflowId);
        }

        return base.OnDisconnectedAsync(exception);
    }

    private bool TryGetWorkflowId(out string? workflowId)
    {
        var headers = Context.GetHttpContext()?.Request.Headers;
        if (headers != null && headers.TryGetValue("Masa-Workflow-Id", out var id))
        {
            workflowId = id;
            return true;
        }

        workflowId = null;
        return false;
    }
}

public interface IDebugHub
{
    Task Log(DebugResult value);
}
