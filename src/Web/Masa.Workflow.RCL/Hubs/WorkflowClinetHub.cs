using Microsoft.AspNetCore.SignalR;

namespace Masa.Workflow.RCL.Hubs;

public class WorkflowClientHub : Hub
{
    public async Task UpdateMqttInState(Guid activityId, string state)
    {
        await Clients.All.SendAsync("WatchMqttInState", activityId, state);
    }
}
