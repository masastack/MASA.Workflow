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
        var workflowDefinition = await _workflowAgentClient.GetDefinitionAsync(new WorkflowId { Id = command.WorkflowId });
        if (workflowDefinition == null)
        {
            throw new UserFriendlyException($"The workflow with id {command.WorkflowId} does not exist");
        }

        //todo change mapster
        var workflowInstance = Convert(workflowDefinition);
        await Console.Out.WriteLineAsync("--------------WorkflowInstance");
        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(workflowInstance));

        var workflowResponse = await _daprClient.StartWorkflowAsync(DaprWorkflowComponent, nameof(MasaWorkFlow), command.WorkflowId, workflowInstance);

    }

    private WorkflowInstance Convert(WorkflowDefinition workflowDefinition)
    {
        var workflowInstance = new WorkflowInstance()
        {
            Id = Guid.Parse(workflowDefinition.Id),
            Name = workflowDefinition.Name,
            StartActivity = new StartActivity(Guid.Parse(workflowDefinition.Nodes.First().Id))
        };

        foreach (var node in workflowDefinition.Nodes)
        {
            var meta = JsonSerializer.Deserialize<JsonObject>(node.Meta);
            //db save meta data not contains ActivityId and Wires
            meta.Add("ActivityId", node.Id);
            var wires = new JsonArray();
            foreach (var wire in node.Wires)
            {
                var wireArray = new JsonArray();
                foreach (var wireId in wire.Guids)
                {
                    wireArray.Add(wireId);
                }
                wires.Add(wireArray);
            }
            meta.Add("Wires", wires);
            workflowInstance.Activities.Add(new ActivityDefinition
            {
                Id = Guid.Parse(node.Id),
                Name = node.Name,
                Disabled = node.Disabled,
                Type = node.Type,
                RetryPolicy = node.RetryPolicy.Adapt<Core.Models.RetryPolicy>(),
                Meta = meta
            });
        }

        return workflowInstance;
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
