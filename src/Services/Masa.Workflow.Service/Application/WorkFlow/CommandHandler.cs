using Masa.Workflow.Service.Application.WorkFlow.Commands;

namespace Masa.Workflow.Service.Application.WorkFlow;

public class CommandHandler
{
    private readonly WorkflowDomainService _domainService;

    public CommandHandler(WorkflowDomainService domainService)
    {
        _domainService = domainService;
    }

    [EventHandler(Order = 1)]
    public async Task CreateHandleAsync(CreateWorkflowCommand command)
    {
        //todo your work
        await Task.CompletedTask;
    }
}