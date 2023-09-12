using Dapr.Client;
using Masa.Workflow.Core;

namespace Masa.Workflow.Service.Application.Orders;

public class CommandHandler
{
    private readonly WorkflowDomainService _domainService;
    private readonly DaprClient _daprClient;

    const string DaprWorkflowComponent = "dapr";

    public CommandHandler(WorkflowDomainService domainService, DaprClient daprClient)
    {
        _domainService = domainService;
        _daprClient = daprClient;
    }

    [EventHandler(Order = 1)]
    public async Task CreateHandleAsync(CreateWorkflowCommand command)
    {
        //todo your work
        await Task.CompletedTask;
    }

    [EventHandler]
    public async Task StartHandleAsync(StartWorkflowCommand command)
    {

        await _daprClient.StartWorkflowAsync(DaprWorkflowComponent, nameof(MasaWorkFlow), command.WorkflowId.ToString(), command.WorkflowId);
        //todo your work
        await Task.CompletedTask;
    }
}