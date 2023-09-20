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
        var workflowId = CheckId(request.Id);
        var query = new WorkflowDetailQuery(workflowId);
        await _eventBus.PublishAsync(query);
        return query.Result;
    }

    public override async Task<WorkflowDefinition> GetDefinition(WorkflowId request, ServerCallContext context)
    {
        var workflowId = CheckId(request.Id);
        var query = new WorkflowDefinitionQuery(workflowId);
        await _eventBus.PublishAsync(query);
        return query.Result;
    }

    public override Task<WorkflowReply> GetList(WorkflowListRequest request, ServerCallContext context)
    {
        return base.GetList(request, context);
    }

    public override async Task<Empty> Delete(WorkflowId request, ServerCallContext context)
    {
        var workflowId = CheckId(request.Id);
        var command = new DeleteWorkflowCommand(workflowId);
        await _eventBus.PublishAsync(command);
        return new Empty();
    }

    public override Task<WorkflowId> Save(WorkflowRequest request, ServerCallContext context)
    {
        return base.Save(request, context);
    }

    private Guid CheckId(string id)
    {
        if (!Guid.TryParse(id, out Guid workflowId))
        {
            throw new UserFriendlyException("Invalid ID");
        }
        return workflowId;
    }
}