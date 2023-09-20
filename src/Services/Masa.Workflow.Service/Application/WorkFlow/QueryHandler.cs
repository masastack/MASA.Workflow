using System.Text.Json;

namespace Masa.Workflow.Service.Application.WorkFlow;

public class QueryHandler
{
    readonly IWorkflowRepository _workflowRepository;

    public QueryHandler(IWorkflowRepository workflowRepository)
    {
        _workflowRepository = workflowRepository;
    }

    [EventHandler]
    public async Task WorkFlowListHandleAsync(WorkflowListQuery query)
    {
        //todo work
    }

    [EventHandler]
    public async Task WorkFlowDetailHandleAsync(WorkflowDetailQuery query)
    {
        query.Result = new WorkflowDetail
        {
            Id = query.Id.ToString(),
            Name = "Test Flow",
            Description = "Hard Code",
            Status = WorkflowStatus.Idle,
            CreateUser = "admin",

            CreateDateTimeStamp = DateTime.UtcNow.ToTimestamp()
        };
        query.Result.EnvironmentVariables.Add("1", "test");
    }

    [EventHandler]
    public async Task WorkFlowDefinitionHandleAsync(WorkflowDefinitionQuery query)
    {
        query.Result = new WorkflowDefinition
        {
            Id = query.Id.ToString(),
            Name = "Test Flow",
        };
        query.Result.EnvironmentVariables.Add("1", "test");
        query.Result.Nodes.Add(new Node
        {
            Name = "Test Node",
            Disabled = false,
            Meta = JsonSerializer.Serialize(new
            {
                Address = "Address",
                Method = "POST",
            })
        });
    }
}