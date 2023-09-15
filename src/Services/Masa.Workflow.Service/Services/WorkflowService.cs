namespace Masa.Workflow.Service.Services;

public class WorkflowService : WorkflowAgent.WorkflowAgentBase
{
    private readonly IEventBus _eventBus;

    public WorkflowService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public override async Task<WorkflowDetail> GetDetail(WorkflowId request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out Guid workflowId))
        {
            throw new UserFriendlyException("Invalid ID");
        }
        var query = new WorkFlowDetailQuery(workflowId);
        await _eventBus.PublishAsync(query);
        return query.Result;
    }

    public override Task<WorkflowReply> GetList(WorkflowListRequest request, ServerCallContext context)
    {
        return base.GetList(request, context);
    }

    public override Task<StatusResponse> Delete(WorkflowId request, ServerCallContext context)
    {
        return base.Delete(request, context);
    }

    public override Task<WorkflowId> Create(WorkflowRequest request, ServerCallContext context)
    {
        return base.Create(request, context);
    }

}