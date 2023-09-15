namespace Masa.Workflow.Worker.Services;

public class WorkflowStarterService : WorkflowStarter.WorkflowStarterBase
{
    private readonly Empty _empty = new();
    private readonly IEventBus _eventBus;

    public WorkflowStarterService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public override async Task<Empty> Start(WorkflowId request, ServerCallContext context)
    {
        await _eventBus.PublishAsync(new StartWorkflowCommand(request.Id));
        return _empty;
    }

    public override async Task<Empty> Stop(WorkflowId request, ServerCallContext context)
    {
        await _eventBus.PublishAsync(new StopWorkflowCommand(request.Id));
        return _empty;
    }
}
