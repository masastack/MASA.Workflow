namespace Masa.Workflow.Service.Application.WorkFlow;

public class CommandHandler
{
    private readonly WorkflowDomainService _domainService;

    public CommandHandler(WorkflowDomainService domainService)
    {
        _domainService = domainService;
    }

    [EventHandler]
    public async Task SaveHandleAsync(SaveWorkflowCommand command)
    {
        command.Id = await _domainService.SaveAsync(command.Request);
    }

    [EventHandler]
    public async Task DeleteHandleAsync(DeleteWorkflowCommand command)
    {
        await _domainService.DeleteAsync(command.WorkflowId);
    }
}