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

    [EventHandler]
    public async Task UpdateStatusHandleAsync(UpdateWorkflowStatusCommand command)
    {
        if (!Guid.TryParse(command.Request.Id, out Guid workflowId))
        {
            throw new UserFriendlyException("Invalid ID");
        }
        await _domainService.UpdateStatusAsync(workflowId, command.Request.Status);
    }
}