namespace Masa.Workflow.Service.Application.Orders;

public class CommandHandler
{
    private readonly WorkflowDomainService _domainService;

    public CommandHandler(WorkflowDomainService domainService)
    {
        _domainService = domainService;
    }

    [EventHandler(Order = 1)]
    public async Task CreateHandleAsync(WorkFlowCreateCommand command)
    {
        //todo your work
        await Task.CompletedTask;
    }
}