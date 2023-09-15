using System.Text.Json;

namespace Masa.Workflow.Worker.Application;

public class CommandHandler
{
    private readonly DaprClient _daprClient;
    private readonly WorkflowAgent.WorkflowAgentClient _workflowAgentClient;

    const string DaprWorkflowComponent = "dapr";

    public CommandHandler(DaprClient daprClient, WorkflowAgent.WorkflowAgentClient workflowAgentClient)
    {
        _daprClient = daprClient;
        _workflowAgentClient = workflowAgentClient;
    }

    [EventHandler]
    public async Task RunHandleAsync(RunWorkflowCommand command)
    {
        var workflowDetail = await _workflowAgentClient.GetDetailAsync(new WorkflowId { Id = command.WorkflowId });
        if (workflowDetail == null)
        {
            throw new UserFriendlyException($"The workflow with id {command.WorkflowId} does not exist");
        }
        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(workflowDetail));
        //todo workflowDetail convert to WorkflowInstance
        var workflowInstance = new WorkflowInstance();
        await _daprClient.StartWorkflowAsync(DaprWorkflowComponent, nameof(MasaWorkFlow), command.WorkflowId, workflowInstance);
    }

    [EventHandler]
    public async Task StartHandleAsync(StartWorkflowCommand command)
    {

        //todo your work
        await Task.CompletedTask;
    }

    [EventHandler]
    public async Task StopHandleAsync(StopWorkflowCommand command)
    {
        //todo your work
        await Task.CompletedTask;
    }
}
