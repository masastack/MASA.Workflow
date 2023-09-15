namespace Masa.Workflow.Worker.Services;

public class WorkflowRunnerService : WorkflowRunner.WorkflowRunnerBase
{
    private readonly IEventBus _eventBus;

    public WorkflowRunnerService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    /// <summary>
    /// Run Workflow
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<Empty> Run(WorkflowId request, ServerCallContext context)
    {
        await _eventBus.PublishAsync(new RunWorkflowCommand(request.Id));
        return new Empty();
    }
}
